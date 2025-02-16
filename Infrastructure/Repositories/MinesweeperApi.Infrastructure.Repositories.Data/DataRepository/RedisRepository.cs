using StackExchange.Redis;
using Newtonsoft.Json;
using MinesweeperApi.Infrastructure.Data.Context;
using AutoMapper;
using MinesweeperApi.Infrastructure.Data.Entities;

namespace MinesweeperApi.Infrastructure.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _expiry = TimeSpan.FromHours(1);
        private readonly IMapper _mapper;

        public RedisRepository(IRedisContext redisContext, IMapper mapper)
        {
            _database = redisContext.Database;
            this._mapper = mapper;
        }

        public async Task<GameEntity> SetAsync(GameEntity newGame)
        {
            string stringKey = newGame.Id;
            string serializedData = newGame.Game;

            await _database.StringSetAsync(stringKey, serializedData, _expiry);

            return newGame;
        }

        public async Task<GameEntity> GetAsync(Guid gameId)
        {
            string stringId = gameId.ToString();

            if (!await _database.KeyExistsAsync(stringId))
            {
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            }

            var game = await _database.StringGetAsync(stringId);

            GameEntity gameEntity = new GameEntity
            {
                Id = stringId,
                Game = game.ToString()
            };

            return gameEntity;
        }

        public async Task DeleteAsync(Guid gameId)
        {
            string stringId = gameId.ToString();

            if (!await _database.KeyExistsAsync(stringId))
            {
                throw new KeyNotFoundException($"The record with the id '{gameId}' was not found in Redis.");
            }

            await _database.KeyDeleteAsync(stringId);
        }
    }
}
