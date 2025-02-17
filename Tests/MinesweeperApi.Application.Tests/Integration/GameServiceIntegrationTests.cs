using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.Logger;
using Moq;
using Newtonsoft.Json;

namespace MinesweeperApi.Application.GameServiceIntegrationTests
{
    /// <summary>
    /// An in-memory implementation of IRedisRepository for integration testing purposes.
    /// </summary>
    public class InMemoryRedisRepository : IRedisRepository
    {
        // Thread-safe dictionary to simulate Redis storage.
        private readonly ConcurrentDictionary<string, GameEntity> _store = new ConcurrentDictionary<string, GameEntity>();

        /// <summary>
        /// Stores a new game entity in the in-memory store.
        /// </summary>
        /// <param name="newGame">The game entity to store.</param>
        /// <returns>The stored game entity.</returns>
        public Task<GameEntity> SetAsync(GameEntity newGame)
        {
            _store[newGame.Id] = newGame;
            return Task.FromResult(newGame);
        }

        /// <summary>
        /// Retrieves a game entity by its GUID.
        /// </summary>
        /// <param name="gameId">The GUID of the game.</param>
        /// <returns>The game entity if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the game entity is not found.</exception>
        public Task<GameEntity> GetAsync(Guid gameId)
        {
            string key = gameId.ToString();
            if (!_store.ContainsKey(key))
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            return Task.FromResult(_store[key]);
        }

        /// <summary>
        /// Deletes a game entity from the in-memory store.
        /// </summary>
        /// <param name="gameId">The GUID of the game to delete.</param>
        /// <returns>A completed task.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the game entity is not found.</exception>
        public Task DeleteAsync(Guid gameId)
        {
            string key = gameId.ToString();
            if (!_store.ContainsKey(key))
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            _store.TryRemove(key, out var _);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Contains integration tests for the GameService, verifying that game creation and moves work as expected.
    /// </summary>
    [TestClass]
    public class GameServiceIntegrationTests
    {
        private IRedisRepository _redisRepository;
        private IMapper _mapper;
        private GameService _gameService;
        private IAppLogger _appLogger;

        /// <summary>
        /// Initializes the test environment by setting up an in-memory Redis repository, AutoMapper configuration, 
        /// a mocked logger, and the GameService.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _redisRepository = new InMemoryRedisRepository();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GameProfile());
                cfg.AddProfile(new CreateGameProfile());
            });
            _mapper = config.CreateMapper();

            var loggerMock = new Mock<IAppLogger>();
            _appLogger = loggerMock.Object;

            _gameService = new GameService(_redisRepository, _mapper, _appLogger);
        }

        /// <summary>
        /// Tests that a new game can be successfully created with the correct dimensions and initial state.
        /// </summary>
        [TestMethod]
        public async Task CreateNewGameIntegrationTest()
        {
            // Arrange
            var createGameModel = new CreateGameModel
            {
                Width = 8,
                Height = 8,
                MinesCount = 10,
                Completed = false
            };

            // Act
            var game = await _gameService.CreateNewGameAsync(createGameModel);

            // Assert
            Assert.AreNotEqual(Guid.Empty, game.Id, "Идентификатор игры не должен быть пустым");
            Assert.AreEqual(8, game.Width, "Ширина должна быть 8");
            Assert.AreEqual(8, game.Height, "Высота должна быть 8");
            Assert.AreEqual(10, game.MinesCount, "Количество мин должно быть 10");
            Assert.IsFalse(game.Completed, "Игра не должна быть завершена при создании");
            Assert.IsNotNull(game.CurrentField, "Игровое поле не должно быть null");
            Assert.AreEqual(8, game.CurrentField.GetLength(0), "Количество строк в игровом поле должно быть 8");
            Assert.AreEqual(8, game.CurrentField.GetLength(1), "Количество столбцов в игровом поле должно быть 8");
        }

        /// <summary>
        /// Tests the game move functionality by first making a safe move, then repeatedly revealing cells until the game is completed.
        /// Verifies that all cells are revealed and the game is marked as completed.
        /// </summary>
        [TestMethod]
        public async Task MakeMoveIntegrationTest_SafeMove_ThenCompleteGame()
        {
            // Arrange
            var createGameModel = new CreateGameModel
            {
                Width = 3,
                Height = 3,
                MinesCount = 1,
                Completed = false
            };

            var game = await _gameService.CreateNewGameAsync(createGameModel);
            // Initialize the field: all cells as unopened (-1)
            for (int i = 0; i < game.Height; i++)
                for (int j = 0; j < game.Width; j++)
                    game.CurrentField[i, j] = -1;
            // Place a mine at (0,1)
            game.CurrentField[0, 1] = -2;
            game.Completed = false;
            var gameEntity = _mapper.Map<GameEntity>(game);
            await _redisRepository.SetAsync(gameEntity);

            // Act: Make an initial safe move at (0,0)
            var move = new MoveModel { GameId = game.Id, Row = 0, Col = 0 };
            var updatedGame = await _gameService.MakeMove(move);

            // Assert: Check that the move reveals a valid number and game is not yet completed.
            Assert.IsTrue(updatedGame.CurrentField[0, 0] >= 0 && updatedGame.CurrentField[0, 0] <= 8, "Ячейка должна содержать число от 0 до 8");
            Assert.IsFalse(updatedGame.Completed, "Игра не должна быть завершена после первого хода");

            // Continue making moves to reveal all unopened cells.
            int maxIterations = 10, iteration = 0;
            while (!updatedGame.Completed && iteration < maxIterations)
            {
                for (int i = 0; i < updatedGame.Height; i++)
                {
                    for (int j = 0; j < updatedGame.Width; j++)
                    {
                        if (updatedGame.CurrentField[i, j] == -1)
                        {
                            var move2 = new MoveModel { GameId = updatedGame.Id, Row = i, Col = j };
                            updatedGame = await _gameService.MakeMove(move2);
                        }
                    }
                }
                iteration++;
            }

            // Assert: Verify that the game is completed and no cell remains unopened or contains a mine indicator (-2).
            Assert.IsTrue(updatedGame.Completed, "Игра не завершилась после последовательных ходов");
            for (int i = 0; i < updatedGame.Height; i++)
            {
                for (int j = 0; j < updatedGame.Width; j++)
                {
                    Assert.AreNotEqual(-1, updatedGame.CurrentField[i, j], "Все ячейки должны быть открыты");
                    Assert.AreNotEqual(-2, updatedGame.CurrentField[i, j], "Ячейки с минами не должны оставаться неоткрытыми");
                }
            }
        }

        /// <summary>
        /// Tests the winning scenario where revealing all safe cells results in the game being marked as completed,
        /// and all mines are revealed correctly.
        /// </summary>
        [TestMethod]
        public async Task MakeMoveIntegrationTest_WinGame_RevealsAllMines()
        {
            // Arrange
            var createGameModel = new CreateGameModel
            {
                Width = 2,
                Height = 2,
                MinesCount = 1,
                Completed = false
            };

            var game = await _gameService.CreateNewGameAsync(createGameModel);
            // Set up the field: three unopened cells and one mine.
            game.CurrentField[0, 0] = -1;
            game.CurrentField[0, 1] = -1;
            game.CurrentField[1, 0] = -1;
            game.CurrentField[1, 1] = -2;
            var gameEntity = _mapper.Map<GameEntity>(game);
            await _redisRepository.SetAsync(gameEntity);

            // Act: Reveal all safe cells.
            var move1 = new MoveModel { GameId = game.Id, Row = 0, Col = 0 };
            game = await _gameService.MakeMove(move1);
            var move2 = new MoveModel { GameId = game.Id, Row = 0, Col = 1 };
            game = await _gameService.MakeMove(move2);
            var move3 = new MoveModel { GameId = game.Id, Row = 1, Col = 0 };
            game = await _gameService.MakeMove(move3);

            // Assert: Verify the game is marked as completed and the mine is revealed (represented by -4).
            Assert.IsTrue(game.Completed, "Игра должна быть завершена после выигрыша");
            Assert.AreEqual(-4, game.CurrentField[1, 1], "Мина должна быть раскрыта с помощью специального индикатора (-4)");
        }

        /// <summary>
        /// Tests that attempting to make a move after the game is completed results in a KeyNotFoundException,
        /// simulating that the game has been removed from the repository.
        /// </summary>
        [TestMethod]
        public async Task MakeMoveIntegrationTest_MoveAfterCompletion_ShouldThrowKeyNotFound()
        {
            // Arrange
            var createGameModel = new CreateGameModel
            {
                Width = 3,
                Height = 3,
                MinesCount = 1,
                Completed = false
            };

            var game = await _gameService.CreateNewGameAsync(createGameModel);
            // Initialize the field: set all cells as unopened (-1)
            for (int i = 0; i < game.Height; i++)
                for (int j = 0; j < game.Width; j++)
                    game.CurrentField[i, j] = -1;
            // Place a mine at (1,1)
            game.CurrentField[1, 1] = -2;
            var gameEntity = _mapper.Map<GameEntity>(game);
            await _redisRepository.SetAsync(gameEntity);

            // Act: Make a move on the mine to complete the game.
            var moveMine = new MoveModel { GameId = game.Id, Row = 1, Col = 1 };
            var updatedGame = await _gameService.MakeMove(moveMine);
            Assert.IsTrue(updatedGame.Completed, "Игра должна быть завершена после хода на мину");

            // Assert: Attempting to make any further move should throw a KeyNotFoundException.
            var moveAfterCompletion = new MoveModel { GameId = game.Id, Row = 0, Col = 0 };
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _gameService.MakeMove(moveAfterCompletion);
            });
        }
    }
}
