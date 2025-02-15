using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;

namespace MinesweeperApi.API;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection service)
    {
        service
            .AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            ;

        return service;
    }
}