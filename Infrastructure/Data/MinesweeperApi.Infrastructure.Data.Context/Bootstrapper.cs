using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Infrastructure.Data.Context;

public static class Bootstrapper
{
    public static IServiceCollection AddRedisContext(this IServiceCollection services)
    {
        services.AddSingleton<IRedisContext, RedisContext>();
        return services;
    }
}
