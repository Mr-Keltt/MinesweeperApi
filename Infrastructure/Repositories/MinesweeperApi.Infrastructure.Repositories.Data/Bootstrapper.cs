using Microsoft.Extensions.DependencyInjection;
using MinesweeperApi.Infrastructure.Data.Context;

namespace MinesweeperApi.Infrastructure.Repositories
{
    /// <summary>
    /// Provides extension methods for configuring Redis-related services in the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Adds Redis context and repository services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the Redis services will be added.</param>
        /// <returns>The updated service collection containing the Redis services.</returns>
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            // Register the Redis context for accessing the Redis database.
            services.AddRedisContext();

            // Register the Redis repository for performing CRUD operations on GameEntity objects.
            services.AddScoped<IRedisRepository, RedisRepository>();

            return services;
        }
    }
}
