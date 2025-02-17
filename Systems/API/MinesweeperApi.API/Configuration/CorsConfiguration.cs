using MinesweeperApi.Application.Services.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MinesweeperApi.API.Configuration
{
    /// <summary>
    /// Provides extension methods for configuring Cross-Origin Resource Sharing (CORS) in the MinesweeperApi application.
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        /// Adds CORS services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which CORS services will be added.</param>
        /// <returns>The updated service collection with CORS services registered.</returns>
        public static IServiceCollection AddAppCors(this IServiceCollection services)
        {
            // Add CORS services to the dependency injection container.
            services.AddCors();
            return services;
        }

        /// <summary>
        /// Configures and enables CORS middleware in the application's request pipeline.
        /// </summary>
        /// <param name="app">The web application to configure.</param>
        public static void UseAppCors(this WebApplication app)
        {
            // Retrieve the main settings from the DI container.
            var mainSettings = app.Services.GetService<MainSettings>();

            // Split the allowed origins based on common delimiters, trim any extra whitespace, and filter out empty entries.
            var origins = mainSettings.AllowedOrigins.Split(',', ';')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            // Configure the CORS policy.
            app.UseCors(policy =>
            {
                // Allow any header and method.
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();

                // Allow credentials.
                policy.AllowCredentials();

                // If specific origins are defined, allow only those origins; otherwise, allow any origin.
                if (origins.Length > 0)
                    policy.WithOrigins(origins);
                else
                    policy.SetIsOriginAllowed(origin => true);

                // Expose the "Content-Disposition" header to the client.
                policy.WithExposedHeaders("Content-Disposition");
            });
        }
    }
}
