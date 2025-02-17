using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Application.Services.GameService
{
    /// <summary>
    /// Provides extension methods for configuring game service related dependencies in the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers the game service implementation with the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the game service is added.</param>
        /// <returns>The updated service collection with the game service registered.</returns>
        public static IServiceCollection AddGameService(this IServiceCollection services)
        {
            // Register IGameService with its concrete implementation GameService as scoped.
            services.AddScoped<IGameService, GameService>();
            return services;
        }
    }
}
