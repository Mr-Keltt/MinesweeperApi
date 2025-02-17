using StackExchange.Redis;

namespace MinesweeperApi.Infrastructure.Data.Context
{
    /// <summary>
    /// Defines a contract for a Redis context which provides access to a Redis database.
    /// </summary>
    public interface IRedisContext
    {
        /// <summary>
        /// Gets the Redis database instance for executing commands against the Redis data store.
        /// </summary>
        IDatabase Database { get; }
    }
}
