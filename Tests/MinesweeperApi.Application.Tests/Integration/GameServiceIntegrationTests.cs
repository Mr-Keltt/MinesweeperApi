using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.Logger;
using Moq;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MinesweeperApi.Application.GameServiceIntegrationTests
{
    public class InMemoryRedisRepository : IRedisRepository
    {
        private readonly ConcurrentDictionary<string, GameEntity> _store = new ConcurrentDictionary<string, GameEntity>();

        public Task<GameEntity> SetAsync(GameEntity newGame)
        {
            _store[newGame.Id] = newGame;
            return Task.FromResult(newGame);
        }

        public Task<GameEntity> GetAsync(Guid gameId)
        {
            string key = gameId.ToString();
            if (!_store.ContainsKey(key))
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            return Task.FromResult(_store[key]);
        }

        public Task DeleteAsync(Guid gameId)
        {
            string key = gameId.ToString();
            if (!_store.ContainsKey(key))
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            _store.TryRemove(key, out var _);
            return Task.CompletedTask;
        }
    }

    [TestClass]
    public class GameServiceIntegrationTests
    {
        private IRedisRepository _redisRepository;
        private IMapper _mapper;
        private GameService _gameService;
        private IAppLogger _appLogger;

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
            Assert.AreNotEqual(Guid.Empty, game.Id);
            Assert.AreEqual(8, game.Width);
            Assert.AreEqual(8, game.Height);
            Assert.AreEqual(10, game.MinesCount);
            Assert.IsFalse(game.Completed);
            Assert.IsNotNull(game.CurrentField);
            Assert.AreEqual(8, game.CurrentField.GetLength(0));
            Assert.AreEqual(8, game.CurrentField.GetLength(1));
        }

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
            for (int i = 0; i < game.Height; i++)
                for (int j = 0; j < game.Width; j++)
                    game.CurrentField[i, j] = -1;
            game.CurrentField[0, 1] = -2;
            game.Completed = false;
            var gameEntity = _mapper.Map<GameEntity>(game);
            await _redisRepository.SetAsync(gameEntity);

            // Act
            var move = new MoveModel { GameId = game.Id, Row = 0, Col = 0 };
            var updatedGame = await _gameService.MakeMove(move);

            // Assert
            Assert.IsTrue(updatedGame.CurrentField[0, 0] >= 0 && updatedGame.CurrentField[0, 0] <= 8);
            Assert.IsFalse(updatedGame.Completed);

            // Act
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

            // Assert
            Assert.IsTrue(updatedGame.Completed, "Game did not complete");
            for (int i = 0; i < updatedGame.Height; i++)
                for (int j = 0; j < updatedGame.Width; j++)
                {
                    Assert.AreNotEqual(-1, updatedGame.CurrentField[i, j]);
                    Assert.AreNotEqual(-2, updatedGame.CurrentField[i, j]);
                }
        }

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
            game.CurrentField[0, 0] = -1;
            game.CurrentField[0, 1] = -1;
            game.CurrentField[1, 0] = -1;

            game.CurrentField[1, 1] = -2;
            var gameEntity = _mapper.Map<GameEntity>(game);
            await _redisRepository.SetAsync(gameEntity);

            // Act
            var move1 = new MoveModel { GameId = game.Id, Row = 0, Col = 0 };
            game = await _gameService.MakeMove(move1);
            var move2 = new MoveModel { GameId = game.Id, Row = 0, Col = 1 };
            game = await _gameService.MakeMove(move2);
            var move3 = new MoveModel { GameId = game.Id, Row = 1, Col = 0 };
            game = await _gameService.MakeMove(move3);

            // Assert
            Assert.IsTrue(game.Completed);
            Assert.AreEqual(-4, game.CurrentField[1, 1]);
        }

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
            for (int i = 0; i < game.Height; i++)
                for (int j = 0; j < game.Width; j++)
                    game.CurrentField[i, j] = -1;
            game.CurrentField[1, 1] = -2;
            var gameEntity = _mapper.Map<GameEntity>(game);
            await _redisRepository.SetAsync(gameEntity);

            // Act
            var moveMine = new MoveModel { GameId = game.Id, Row = 1, Col = 1 };
            var updatedGame = await _gameService.MakeMove(moveMine);
            Assert.IsTrue(updatedGame.Completed);

            // Assert
            var moveAfterCompletion = new MoveModel { GameId = game.Id, Row = 0, Col = 0 };
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _gameService.MakeMove(moveAfterCompletion);
            });
        }
    }
}
