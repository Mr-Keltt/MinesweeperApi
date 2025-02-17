using AutoMapper;
using Moq;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.Logger;
using System;
using System.Threading.Tasks;

namespace MinesweeperApi.Application.GameServiceUnitTests
{
    [TestClass]
    public class GameServiceUnitTests
    {
        public TestContext TestContext { get; set; }
        private Mock<IRedisRepository> _redisRepositoryMock;
        private IMapper _mapper;
        private Mock<IAppLogger> _appLoggerMock;
        private GameService _gameService;

        [TestInitialize]
        public void Setup()
        {
            TestContext.WriteLine("Setup started.");
            _redisRepositoryMock = new Mock<IRedisRepository>();
            _appLoggerMock = new Mock<IAppLogger>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GameProfile());
                cfg.AddProfile(new CreateGameProfile());
            });
            _mapper = config.CreateMapper();
            _gameService = new GameService(_redisRepositoryMock.Object, _mapper, _appLoggerMock.Object);
            TestContext.WriteLine("Setup finished.");
        }

        [TestMethod]
        public async Task CreateNewGameAsync_WithValidParameters_ShouldReturnNewGame()
        {
            TestContext.WriteLine("Test: CreateNewGameAsync_WithValidParameters_ShouldReturnNewGame");
            // Arrange
            var createGameModel = new CreateGameModel
            {
                Width = 10,
                Height = 10,
                MinesCount = 10,
                Completed = false
            };

            _redisRepositoryMock
                .Setup(r => r.SetAsync(It.IsAny<GameEntity>()))
                .ReturnsAsync((GameEntity g) => g);

            // Act
            var game = await _gameService.CreateNewGameAsync(createGameModel);
            TestContext.WriteLine($"Game created: Width={game.Width}, Height={game.Height}, MinesCount={game.MinesCount}");

            // Assert
            Assert.IsNotNull(game);
            Assert.AreEqual(10, game.Width);
            Assert.AreEqual(10, game.Height);
            Assert.AreEqual(10, game.MinesCount);
            Assert.IsFalse(game.Completed);
            Assert.IsNotNull(game.CurrentField);
            Assert.AreEqual(10, game.CurrentField.GetLength(0));
            Assert.AreEqual(10, game.CurrentField.GetLength(1));
        }

        [DataTestMethod]
        [DataRow(0, 10, 10)]
        [DataRow(100, 10, 10)]
        public async Task CreateNewGameAsync_WithInvalidMinesCount_ShouldThrowArgumentException(int minesCount, int width, int height)
        {
            TestContext.WriteLine("Test: CreateNewGameAsync_WithInvalidMinesCount_ShouldThrowArgumentException");
            // Arrange
            var createGameModel = new CreateGameModel
            {
                Width = width,
                Height = height,
                MinesCount = minesCount,
                Completed = false
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _gameService.CreateNewGameAsync(createGameModel);
            });
        }

        [TestMethod]
        public async Task MakeMove_WithValidSafeMove_ShouldRevealFieldAndUpdateGame()
        {
            TestContext.WriteLine("Test: MakeMove_WithValidSafeMove_ShouldRevealFieldAndUpdateGame");
            // Arrange
            var gameModel = new GameModel
            {
                Id = Guid.NewGuid(),
                Width = 5,
                Height = 5,
                MinesCount = 3,
                Completed = false,
                CurrentField = new int[5, 5]
            };

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    gameModel.CurrentField[i, j] = -1;
            gameModel.CurrentField[1, 1] = -2;
            gameModel.CurrentField[2, 2] = -2;
            gameModel.CurrentField[3, 3] = -2;

            var gameEntity = new GameEntity
            {
                Id = gameModel.Id.ToString(),
                Game = JsonConvert.SerializeObject(gameModel)
            };

            _redisRepositoryMock
                .Setup(r => r.GetAsync(gameModel.Id))
                .ReturnsAsync(gameEntity);
            _redisRepositoryMock
                .Setup(r => r.SetAsync(It.IsAny<GameEntity>()))
                .ReturnsAsync((GameEntity g) => g);

            // Act
            var move = new MoveModel { GameId = gameModel.Id, Row = 0, Col = 0 };
            var updatedGame = await _gameService.MakeMove(move);
            TestContext.WriteLine("After move at (0,0)");

            // Assert
            Assert.IsTrue(updatedGame.CurrentField[0, 0] >= 0 && updatedGame.CurrentField[0, 0] <= 8);
            Assert.IsFalse(updatedGame.Completed);
        }

        [TestMethod]
        public async Task MakeMove_OnMine_ShouldCompleteGameAndDeleteFromRepository()
        {
            TestContext.WriteLine("Test: MakeMove_OnMine_ShouldCompleteGameAndDeleteFromRepository");
            // Arrange
            var gameModel = new GameModel
            {
                Id = Guid.NewGuid(),
                Width = 5,
                Height = 5,
                MinesCount = 1,
                Completed = false,
                CurrentField = new int[5, 5]
            };

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    gameModel.CurrentField[i, j] = -1;
            gameModel.CurrentField[2, 2] = -2;

            var gameEntity = new GameEntity
            {
                Id = gameModel.Id.ToString(),
                Game = JsonConvert.SerializeObject(gameModel)
            };

            _redisRepositoryMock
                .Setup(r => r.GetAsync(gameModel.Id))
                .ReturnsAsync(gameEntity);
            _redisRepositoryMock
                .Setup(r => r.DeleteAsync(gameModel.Id))
                .Returns(Task.CompletedTask);
            _redisRepositoryMock
                .Setup(r => r.SetAsync(It.IsAny<GameEntity>()))
                .ReturnsAsync((GameEntity g) => g);

            // Act
            var move = new MoveModel { GameId = gameModel.Id, Row = 2, Col = 2 };
            var updatedGame = await _gameService.MakeMove(move);
            TestContext.WriteLine("After move at (2,2)");

            // Assert
            Assert.IsTrue(updatedGame.Completed);
            Assert.AreEqual(-3, updatedGame.CurrentField[2, 2]);
            _redisRepositoryMock.Verify(r => r.DeleteAsync(gameModel.Id), Times.Once);
        }

        [DataTestMethod]
        [DataRow(-1, 0)]
        [DataRow(0, -1)]
        [DataRow(5, 0)]
        [DataRow(0, 5)]
        public async Task MakeMove_WithOutOfBoundsCoordinates_ShouldThrowArgumentException(int row, int col)
        {
            TestContext.WriteLine("Test: MakeMove_WithOutOfBoundsCoordinates_ShouldThrowArgumentException");
            // Arrange
            var gameModel = new GameModel
            {
                Id = Guid.NewGuid(),
                Width = 5,
                Height = 5,
                MinesCount = 1,
                Completed = false,
                CurrentField = new int[5, 5]
            };

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    gameModel.CurrentField[i, j] = -1;

            var gameEntity = new GameEntity
            {
                Id = gameModel.Id.ToString(),
                Game = JsonConvert.SerializeObject(gameModel)
            };

            _redisRepositoryMock
                .Setup(r => r.GetAsync(gameModel.Id))
                .ReturnsAsync(gameEntity);

            // Act & Assert
            var move = new MoveModel { GameId = gameModel.Id, Row = row, Col = col };
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _gameService.MakeMove(move);
            });
        }

        [TestMethod]
        public async Task MakeMove_OnAlreadyOpenedCell_ShouldThrowArgumentException()
        {
            // Arrange
            var gameModel = new GameModel
            {
                Id = Guid.NewGuid(),
                Width = 3,
                Height = 3,
                MinesCount = 1,
                Completed = false,
                CurrentField = new int[3, 3]
            };

            // Инициализируем все ячейки как закрытые (-1) и вручную "открываем" клетку (1,1)
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    gameModel.CurrentField[i, j] = -1;
            gameModel.CurrentField[1, 1] = 1;

            var gameEntity = new GameEntity
            {
                Id = gameModel.Id.ToString(),
                Game = JsonConvert.SerializeObject(gameModel)
            };

            _redisRepositoryMock
                .Setup(r => r.GetAsync(gameModel.Id))
                .ReturnsAsync(gameEntity);

            // Act & Assert
            var move = new MoveModel { GameId = gameModel.Id, Row = 1, Col = 1 };
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _gameService.MakeMove(move);
            }, "Ячейка уже открыта.");
        }
    }
}
