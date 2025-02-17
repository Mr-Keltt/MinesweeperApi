using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.GameService;

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
            .AddGameService()
            ;

        return service;
    }
}