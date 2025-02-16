using StackExchange.Redis;

namespace MinesweeperApi.Infrastructure.Data.Context;

public interface IRedisContext
{
    IDatabase Database { get; }
}