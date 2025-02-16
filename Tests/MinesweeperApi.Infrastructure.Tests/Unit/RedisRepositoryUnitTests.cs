﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StackExchange.Redis;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Infrastructure.Data.Context;
using MinesweeperApi.Infrastructure.Data.Entities;

namespace MinesweeperApi.Tests.Repositories
{
    [TestClass]
    public class RedisRepositoryTests
    {
        private Mock<IDatabase> _mockDatabase;
        private Mock<IRedisContext> _mockRedisContext;
        private RedisRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _mockDatabase = new Mock<IDatabase>();
            _mockRedisContext = new Mock<IRedisContext>();
            _mockRedisContext.SetupGet(x => x.Database).Returns(_mockDatabase.Object);

            _repository = new RedisRepository(_mockRedisContext.Object, null);
        }

        [TestMethod]
        public async Task SetAsync_Should_Save_Record_And_Return_GameEntity()
        {
            // Arrange
            var gameEntity = new GameEntity
            {
                Id = "TestId",
                Game = "{\"some\":\"json\"}"
            };

            _mockDatabase
                .Setup(db => db.StringSetAsync(
                    It.IsAny<RedisKey>(),
                    It.IsAny<RedisValue>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            // Act
            GameEntity result = await _repository.SetAsync(gameEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(gameEntity.Id, result.Id);
            Assert.AreEqual(gameEntity.Game, result.Game);

            _mockDatabase.Verify(db => db.StringSetAsync(
                gameEntity.Id,
                gameEntity.Game,
                It.IsAny<TimeSpan>(),
                false,
                When.Always,
                CommandFlags.None), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_Should_Return_GameEntity_When_Record_Exists()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            string stringId = testGuid.ToString();
            string storedJson = "{\"some\":\"json\"}";

            _mockDatabase.Setup(db => db.KeyExistsAsync(stringId, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(true);
            _mockDatabase.Setup(db => db.StringGetAsync(stringId, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(storedJson);

            // Act
            GameEntity result = await _repository.GetAsync(testGuid);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(stringId, result.Id);
            Assert.AreEqual(storedJson, result.Game);

            _mockDatabase.Verify(db => db.KeyExistsAsync(stringId, It.IsAny<CommandFlags>()), Times.Once);
            _mockDatabase.Verify(db => db.StringGetAsync(stringId, It.IsAny<CommandFlags>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetAsync_Should_Throw_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            string stringId = testGuid.ToString();

            _mockDatabase.Setup(db => db.KeyExistsAsync(stringId, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(false);

            // Act
            await _repository.GetAsync(testGuid);
        }

        [TestMethod]
        public async Task DeleteAsync_Should_Delete_Record_When_Record_Exists()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            string stringId = testGuid.ToString();

            _mockDatabase.Setup(db => db.KeyExistsAsync(stringId, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(true);
            _mockDatabase.Setup(db => db.KeyDeleteAsync(stringId, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(true);

            // Act
            await _repository.DeleteAsync(testGuid);

            // Assert
            _mockDatabase.Verify(db => db.KeyExistsAsync(stringId, It.IsAny<CommandFlags>()), Times.Once);
            _mockDatabase.Verify(db => db.KeyDeleteAsync(stringId, It.IsAny<CommandFlags>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task DeleteAsync_Should_Throw_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            Guid testGuid = Guid.NewGuid();
            string stringId = testGuid.ToString();

            _mockDatabase.Setup(db => db.KeyExistsAsync(stringId, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(false);

            // Act
            await _repository.DeleteAsync(testGuid);
        }
    }
}
