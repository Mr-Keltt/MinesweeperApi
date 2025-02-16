using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StackExchange.Redis;
using MinesweeperApi.Infrastructure.Data.Context;
using MinesweeperApi.Infrastructure.Repositories;

namespace MinesweeperApi.Infrastructure.Tests
{
    [TestClass]
    public class RedisRepositoryUnitTests
    {
        private Mock<IRedisContext> _redisContextMock;
        private Mock<IDatabase> _databaseMock;
        private RedisRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            _databaseMock = new Mock<IDatabase>();
            _redisContextMock = new Mock<IRedisContext>();

            _redisContextMock
                .SetupGet(r => r.Database)
                .Returns(_databaseMock.Object);

            _repository = new RedisRepository(_redisContextMock.Object);
        }

        [TestMethod]
        public async Task SetAsync_ValidData_CallsStringSetAsyncWithCorrectParameters()
        {
            // Arrange
            var key = Guid.NewGuid();
            var data = new int[,] { { 1, 2 }, { 3, 4 } };

            // Act
            await _repository.SetAsync(key, data);

            // Assert
            _databaseMock.Verify(db =>
                db.StringSetAsync(
                    key.ToString(),
                    It.IsAny<RedisValue>(),
                    TimeSpan.FromHours(1),
                    false,
                    When.Always,
                    CommandFlags.None),
                Times.Once
            );
        }

        [TestMethod]
        public async Task GetAsync_KeyExists_ReturnsDeserializedArray()
        {
            // Arrange
            var key = Guid.NewGuid();
            var expected = new int[,] { { 1, 2 }, { 3, 4 } };
            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(expected);

            _databaseMock
                .Setup(db => db.StringGetAsync(key.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(serialized);

            // Act
            var result = await _repository.GetAsync(key);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected[0, 0], result[0, 0]);
            Assert.AreEqual(expected[1, 1], result[1, 1]);
        }

        [TestMethod]
        public async Task GetAsync_KeyNotExists_ReturnsNull()
        {
            // Arrange
            var key = Guid.NewGuid();

            _databaseMock
                .Setup(db => db.StringGetAsync(key.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue)RedisValue.Null);

            // Act
            var result = await _repository.GetAsync(key);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ValidKey_DeletesKeyAndReturnsTrue()
        {
            // Arrange
            var key = Guid.NewGuid();
            _databaseMock
                .Setup(db => db.KeyDeleteAsync(key.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            // Act
            var deleted = await _repository.DeleteAsync(key);

            // Assert
            Assert.IsTrue(deleted);
            _databaseMock.Verify(db => db.KeyDeleteAsync(key.ToString(), CommandFlags.None), Times.Once);
        }
    }
}
