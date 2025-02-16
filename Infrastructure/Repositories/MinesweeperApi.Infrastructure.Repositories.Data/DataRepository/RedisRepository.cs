using StackExchange.Redis;
using Newtonsoft.Json;
using MinesweeperApi.Infrastructure.Data.Context;

namespace MinesweeperApi.Infrastructure.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _expiry = TimeSpan.FromHours(1);

        public RedisRepository(IRedisContext redisContext)
        {
            _database = redisContext.Database;
        }

        public async Task SetAsync(Guid key, int[,] data)
        {
            string serializedData = JsonConvert.SerializeObject(data);
            await _database.StringSetAsync(key.ToString(), serializedData, _expiry);
        }

        public async Task<int[,]> GetAsync(Guid key)
        {
            var data = await _database.StringGetAsync(key.ToString());
            return data.HasValue ? JsonConvert.DeserializeObject<int[,]>(data) : null;
        }

        public async Task<bool> DeleteAsync(Guid key)
        {
            return await _database.KeyDeleteAsync(key.ToString());
        }
    }
}
