namespace MinesweeperApi.Infrastructure.Data.Context;

using StackExchange.Redis;
using MinesweeperApi.Application.Services.Settings;
using MinesweeperApi.Common.Settings;
using System;

public class RedisContext : IRedisContext, IDisposable
{
    private readonly ConnectionMultiplexer _connection;
    public virtual IDatabase Database { get; }

    public RedisContext()
    {
        var redisSettings = Settings.Load<RedisSettings>("Redis");
        _connection = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        Database = _connection.GetDatabase();
    }

    public void Dispose()
    {
        _connection.Close();
    }
}