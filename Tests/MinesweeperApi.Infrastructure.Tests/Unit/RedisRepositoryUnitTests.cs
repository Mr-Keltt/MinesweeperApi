using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StackExchange.Redis;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Infrastructure.Data.Context;
using MinesweeperApi.Infrastructure.Data.Entities;

namespace MinesweeperApi.Infrastructure.RedisRepositoryUnitTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="RedisRepository"/> class.
    /// These tests verify that the repository correctly sets, retrieves, and deletes game records in Redis.
    /// </summary>
    [TestClass]
    public class RedisRepositoryTests
    {
        private Mock<IDatabase> _mockDatabase;
        private Mock<IRedisContext> _mockRedisContext;
        private RedisRepository _repository;

        /// <summary>
        /// Initializes the test environment before each test.
        /// Sets up the mocked Redis database and context, and creates a new instance of the RedisRepository.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockDatabase = new Mock<IDatabase>();
            _mockRedisContext = new Mock<IRedisContext>();
            _mockRedisContext.SetupGet(x => x.Database).Returns(_mockDatabase.Object);

            // The second parameter (mapper) is set to null for these tests.
            _repository = new RedisRepository(_mockRedisContext.Object, null);
        }

        /// <summary>
        /// Tests that calling SetAsync saves a game entity to Redis and returns the same entity.
        /// </summary>
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
                    It.IsAny<bool>(),
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

        /// <summary>
        /// Tests that GetAsync returns a game entity when the record exists in Redis.
        /// </summary>
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

        /// <summary>
        /// Tests that GetAsync throws a KeyNotFoundException when the requested record does not exist in Redis.
        /// </summary>
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

        /// <summary>
        /// Tests that DeleteAsync successfully deletes a record when it exists in Redis.
        /// </summary>
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

        /// <summary>
        /// Tests that DeleteAsync throws a KeyNotFoundException when attempting to delete a record that does not exist in Redis.
        /// </summary>
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
