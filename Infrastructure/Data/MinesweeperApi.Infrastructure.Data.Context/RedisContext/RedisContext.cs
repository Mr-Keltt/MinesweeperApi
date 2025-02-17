namespace MinesweeperApi.Infrastructure.Data.Context
{
    using StackExchange.Redis;
    using MinesweeperApi.Application.Services.Settings;
    using MinesweeperApi.Common.Settings;
    using System;

    /// <summary>
    /// Provides a context for connecting to and interacting with a Redis database.
    /// Implements the <see cref="IRedisContext"/> interface for accessing the database and 
    /// <see cref="IDisposable"/> for proper resource cleanup.
    /// </summary>
    public class RedisContext : IRedisContext, IDisposable
    {
        // Holds the connection instance to the Redis server.
        private readonly ConnectionMultiplexer _connection;

        /// <summary>
        /// Gets the Redis database instance for executing Redis commands.
        /// </summary>
        public virtual IDatabase Database { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisContext"/> class.
        /// Loads the Redis configuration settings, establishes a connection to the Redis server,
        /// and retrieves the corresponding database instance.
        /// </summary>
        public RedisContext()
        {
            // Load Redis settings from configuration using the settings service.
            var redisSettings = Settings.Load<RedisSettings>("Redis");

            // Establish a connection to the Redis server using the provided connection string.
            _connection = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);

            // Retrieve the default Redis database for executing commands.
            Database = _connection.GetDatabase();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="RedisContext"/> instance.
        /// Closes the connection to the Redis server.
        /// </summary>
        public void Dispose()
        {
            // Close the Redis connection when disposing of the context.
            _connection.Close();
        }
    }
}
