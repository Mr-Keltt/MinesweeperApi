using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MinesweeperApi.Infrastructure.Data.Context;
using MinesweeperApi.Infrastructure.Data.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MinesweeperApi.Infrastructure.Repositories
{
    /// <summary>
    /// Implements repository operations for <see cref="GameEntity"/> objects in a Redis data store.
    /// </summary>
    public class RedisRepository : IRedisRepository
    {
        // The Redis database instance for performing data operations.
        private readonly IDatabase _database;

        // Defines the expiration duration for the stored records.
        private readonly TimeSpan _expiry = TimeSpan.FromHours(1);

        // The AutoMapper instance for mapping between different object models if needed.
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisRepository"/> class.
        /// </summary>
        /// <param name="redisContext">The Redis context that provides access to the Redis database.</param>
        /// <param name="mapper">The mapper used for object-object mapping.</param>
        public RedisRepository(IRedisContext redisContext, IMapper mapper)
        {
            _database = redisContext.Database;
            _mapper = mapper;
        }

        /// <summary>
        /// Asynchronously stores or updates a <see cref="GameEntity"/> in the Redis data store.
        /// </summary>
        /// <param name="newGame">The game entity to store in Redis.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the stored <see cref="GameEntity"/>.
        /// </returns>
        public async Task<GameEntity> SetAsync(GameEntity newGame)
        {
            // Use the game entity's Id as the Redis key.
            string stringKey = newGame.Id;

            // Use the game's serialized data directly.
            string serializedData = newGame.Game;

            // Store the serialized game data in Redis with the specified expiry time.
            await _database.StringSetAsync(stringKey, serializedData, _expiry);

            return newGame;
        }

        /// <summary>
        /// Asynchronously retrieves a <see cref="GameEntity"/> from Redis using its unique identifier.
        /// </summary>
        /// <param name="gameId">The unique identifier of the game entity.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the retrieved <see cref="GameEntity"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the specified game entity does not exist in Redis.
        /// </exception>
        public async Task<GameEntity> GetAsync(Guid gameId)
        {
            // Convert the GUID to a string to match the Redis key format.
            string stringId = gameId.ToString();

            // Check if the key exists in the Redis data store.
            if (!await _database.KeyExistsAsync(stringId))
            {
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            }

            // Retrieve the serialized game data from Redis.
            var game = await _database.StringGetAsync(stringId);

            // Create a GameEntity with the retrieved data.
            GameEntity gameEntity = new GameEntity
            {
                Id = stringId,
                Game = game.ToString()
            };

            return gameEntity;
        }

        /// <summary>
        /// Asynchronously deletes a <see cref="GameEntity"/> from Redis using its unique identifier.
        /// </summary>
        /// <param name="gameId">The unique identifier of the game entity to delete.</param>
        /// <returns>
        /// A task representing the asynchronous deletion operation.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the specified game entity does not exist in Redis.
        /// </exception>
        public async Task DeleteAsync(Guid gameId)
        {
            // Convert the GUID to a string to form the Redis key.
            string stringId = gameId.ToString();

            // Verify that the key exists before attempting deletion.
            if (!await _database.KeyExistsAsync(stringId))
            {
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            }

            // Delete the key and its associated value from Redis.
            await _database.KeyDeleteAsync(stringId);
        }
    }
}
