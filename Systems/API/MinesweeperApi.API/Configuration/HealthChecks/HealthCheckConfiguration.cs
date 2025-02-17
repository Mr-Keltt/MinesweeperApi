namespace MinesweeperApi.API.Configuration
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using MinesweeperApi.Common.HealthCheck;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides extension methods for configuring health checks within the MinesweeperApi application.
    /// </summary>
    public static class HealthCheckConfiguration
    {
        /// <summary>
        /// Registers application health checks with the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the health checks are added.</param>
        /// <returns>The updated service collection with health checks configured.</returns>
        public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
        {
            // Add basic health checks and register a self-check to monitor the API's health.
            services.AddHealthChecks()
                .AddCheck<SelfHealthCheck>("MinesweeperApi.API");

            return services;
        }

        /// <summary>
        /// Configures the health check endpoints for the web application.
        /// </summary>
        /// <param name="app">The web application builder instance.</param>
        public static void UseAppHealthChecks(this WebApplication app)
        {
            // Maps a basic health check endpoint at "/health".
            app.MapHealthChecks("/health");

            // Maps a detailed health check endpoint at "/health/detail" with a custom response writer.
            app.MapHealthChecks("/health/detail", new HealthCheckOptions
            {
                // Writes a detailed health check response using the HealthCheckHelper.
                ResponseWriter = HealthCheckHelper.WriteHealthCheckResponse,

                // Disables caching for the health check responses.
                AllowCachingResponses = false
            });
        }
    }
}
