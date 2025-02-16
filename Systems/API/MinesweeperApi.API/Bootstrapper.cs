using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;
using MinesweeperApi.Infrastructure.Repositories;

namespace MinesweeperApi.API;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection service)
    {
        service
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddRedisSettings()
            .AddAppAutoMappers()
            .AddRedis()
            ;

        return service;
    }
}