namespace MinesweeperApi.Application.Services.Settings
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MinesweeperApi.Common.Settings;

    /// <summary>
    /// Provides extension methods for registering application settings with the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Loads the main application settings from configuration and registers them as a singleton.
        /// </summary>
        /// <param name="services">The service collection to which the main settings will be added.</param>
        /// <param name="configuration">
        /// An optional configuration instance from which to load the settings. If null, the default configuration is used.
        /// </param>
        /// <returns>The updated service collection with the main settings registered.</returns>
        public static IServiceCollection AddMainSettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            // Load the MainSettings section from the configuration using the helper method.
            var settings = Common.Settings.Settings.Load<MainSettings>("Main", configuration);

            // Register the loaded settings as a singleton in the dependency injection container.
            services.AddSingleton(settings);

            return services;
        }

        /// <summary>
        /// Loads the logging settings from configuration and registers them as a singleton.
        /// </summary>
        /// <param name="services">The service collection to which the log settings will be added.</param>
        /// <param name="configuration">
        /// An optional configuration instance from which to load the settings. If null, the default configuration is used.
        /// </param>
        /// <returns>The updated service collection with the log settings registered.</returns>
        public static IServiceCollection AddLogSettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            // Load the LogSettings section from the configuration using the helper method.
            var settings = Common.Settings.Settings.Load<LogSettings>("Log", configuration);

            // Register the loaded settings as a singleton.
            services.AddSingleton(settings);

            return services;
        }

        /// <summary>
        /// Loads the Swagger settings from configuration and registers them as a singleton.
        /// </summary>
        /// <param name="services">The service collection to which the Swagger settings will be added.</param>
        /// <param name="configuration">
        /// An optional configuration instance from which to load the settings. If null, the default configuration is used.
        /// </param>
        /// <returns>The updated service collection with the Swagger settings registered.</returns>
        public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            // Load the SwaggerSettings section from the configuration using the helper method.
            var settings = Common.Settings.Settings.Load<SwaggerSettings>("Swagger", configuration);

            // Register the loaded settings as a singleton.
            services.AddSingleton(settings);

            return services;
        }

        /// <summary>
        /// Loads the Redis settings from configuration and registers them as a singleton.
        /// </summary>
        /// <param name="services">The service collection to which the Redis settings will be added.</param>
        /// <param name="configuration">
        /// An optional configuration instance from which to load the settings. If null, the default configuration is used.
        /// </param>
        /// <returns>The updated service collection with the Redis settings registered.</returns>
        public static IServiceCollection AddRedisSettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            // Load the RedisSettings section from the configuration using the helper method.
            var settings = Common.Settings.Settings.Load<RedisSettings>("Redis", configuration);

            // Register the loaded settings as a singleton.
            services.AddSingleton(settings);

            return services;
        }
    }
}
