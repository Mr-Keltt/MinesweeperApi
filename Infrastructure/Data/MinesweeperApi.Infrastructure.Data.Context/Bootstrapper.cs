using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.Infrastructure.Data.Context
{
    /// <summary>
    /// Provides extension methods for configuring the Redis context within the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Adds the Redis context to the dependency injection container as a singleton.
        /// This allows a single instance of <see cref="IRedisContext"/> to be used throughout the application.
        /// </summary>
        /// <param name="services">The service collection to which the Redis context is added.</param>
        /// <returns>The updated service collection with the Redis context configured.</returns>
        public static IServiceCollection AddRedisContext(this IServiceCollection services)
        {
            // Register IRedisContext with its implementation RedisContext as a singleton.
            services.AddSingleton<IRedisContext, RedisContext>();
            return services;
        }
    }
}
