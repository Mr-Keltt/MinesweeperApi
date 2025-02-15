using MinesweeperApi.API.Configuration;

namespace MinesweeperApi.API;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection service, IConfiguration configuration = null)
    {
        service
            .AddSwaggerSettings()
            ;

        return service;
    }
}