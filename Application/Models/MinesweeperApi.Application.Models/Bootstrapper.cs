using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Application.Models
{
    /// <summary>
    /// Provides extension methods for configuring application models and related services.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Adds AutoMapper profiles for application models to the dependency injection container.
        /// This ensures that model-to-entity mappings are properly configured.
        /// </summary>
        /// <param name="services">The service collection to which the AutoMapper services will be added.</param>
        /// <returns>The updated service collection with the application model services configured.</returns>
        public static IServiceCollection AddApplicationModels(this IServiceCollection services)
        {
            // Register AutoMapper and include the CreateGameProfile for model mappings.
            services.AddAutoMapper(typeof(CreateGameProfile));

            return services;
        }
    }
}
