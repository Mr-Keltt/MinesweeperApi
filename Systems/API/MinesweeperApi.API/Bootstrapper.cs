using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Application.Models;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Services.Logger;

namespace MinesweeperApi.API;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServicesAndModels(this IServiceCollection service)
    {
        service
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddRedisSettings()
            .AddAppLogger()
            .AddAppAutoMappers()
            .AddRedis()
            .AddGameService()
            .AddApplicationModels()
            .AddAutoMapper(typeof(GameResponseProfile))
            ;

        return service;
    }
}