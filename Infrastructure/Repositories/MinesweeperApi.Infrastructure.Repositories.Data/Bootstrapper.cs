using Microsoft.Extensions.DependencyInjection;
using MinesweeperApi.Infrastructure.Data.Context;

namespace MinesweeperApi.Infrastructure.Repositories;

public static class Bootstrapper
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddRedisContext();
        services.AddScoped<IRedisRepository, RedisRepository>();
        return services;
    }
}