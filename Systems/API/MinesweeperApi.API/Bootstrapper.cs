using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Application.Models;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Services.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.API
{
    /// <summary>
    /// Provides extension methods for registering services, models, and configurations used throughout the Minesweeper API.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers application settings, logging, repository, game service, AutoMapper profiles, and other related services
        /// with the dependency injection container.
        /// </summary>
        /// <param name="service">The service collection to which the services and models will be added.</param>
        /// <returns>
        /// The updated service collection with all required services, models, and configurations registered.
        /// </returns>
        public static IServiceCollection RegisterServicesAndModels(this IServiceCollection service)
        {
            service
                // Register the main application settings from configuration.
                .AddMainSettings()
                // Register Swagger settings.
                .AddSwaggerSettings()
                // Register logging settings.
                .AddLogSettings()
                // Register Redis settings.
                .AddRedisSettings()
                // Register the application logger.
                .AddAppLogger()
                // Register AutoMapper profiles defined in the application models.
                .AddAppAutoMappers()
                // Register the Redis repository and context.
                .AddRedis()
                // Register the game service which contains business logic for game operations.
                .AddGameService()
                // Register application models such as AutoMapper profiles related to models.
                .AddApplicationModels()
                // Register additional AutoMapper profiles. This line registers the profile for mapping GameInfoResponse.
                .AddAutoMapper(typeof(GameInfoResponseProfile));

            return service;
        }
    }
}
