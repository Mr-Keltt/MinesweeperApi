namespace MinesweeperApi.Application.Services.Settings
{
    /// <summary>
    /// Represents configuration settings required to connect to a Redis server.
    /// </summary>
    public class RedisSettings
    {
        /// <summary>
        /// Gets the connection string used to establish a connection to the Redis server.
        /// </summary>
        public string ConnectionString { get; private set; }
    }
}
