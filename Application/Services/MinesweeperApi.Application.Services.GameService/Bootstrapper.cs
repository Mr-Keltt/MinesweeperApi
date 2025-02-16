using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Application.Services.GameService;

public static class GameServiceBootstrapper
{
    public static IServiceCollection AddGameService(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        return services;
    }
}
