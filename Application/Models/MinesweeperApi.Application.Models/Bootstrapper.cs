using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Application.Models;

public static class Bootstrapper
{
    public static IServiceCollection AddApplicationModels(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CreateGameProfile));

        return services;
    }
}
