using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Application.Services.Logger
{
    /// <summary>
    /// Provides extension methods to register the application logger with the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Adds the application logger to the service collection as a singleton.
        /// </summary>
        /// <param name="services">The service collection to which the logger is added.</param>
        /// <returns>The updated service collection with the application logger registered.</returns>
        public static IServiceCollection AddAppLogger(this IServiceCollection services)
        {
            // Register IAppLogger with its implementation AppLogger as a singleton.
            return services.AddSingleton<IAppLogger, AppLogger>();
        }
    }
}
