using Microsoft.OpenApi.Models;
using MinesweeperApi.Application.Services.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MinesweeperApi.API.Configuration
{
    /// <summary>
    /// Provides extension methods for configuring Swagger for the Minesweeper API.
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Adds and configures Swagger generation services based on the provided Swagger settings.
        /// </summary>
        /// <param name="services">The service collection to which Swagger services will be added.</param>
        /// <param name="swaggerSettings">The settings that determine if Swagger should be enabled and configured.</param>
        /// <returns>The updated service collection with Swagger services registered if enabled.</returns>
        public static IServiceCollection AddAppSwagger(this IServiceCollection services, SwaggerSettings swaggerSettings)
        {
            // If Swagger is not enabled in the settings, simply return the service collection without configuration.
            if (!swaggerSettings.Enabled)
                return services;

            // Add Swagger generation and configure the Swagger document.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minesweeper API",
                    Version = "v1",
                    Description = "API для игры Minesweeper", // Description in Russian
                    Contact = new OpenApiContact
                    {
                        Name = "Mr-Keltt",
                        Email = "dmitriydavidenkokeltt@gmail.com",
                        Url = new Uri("https://t.me/MrKelttPro")
                    }
                });

                // Add an operation filter to force the use of JSON for request and response bodies.
                c.OperationFilter<ForceJsonOperationFilter>();
            });

            return services;
        }

        /// <summary>
        /// Configures the application to use Swagger and Swagger UI middleware.
        /// </summary>
        /// <param name="app">The application builder to configure.</param>
        /// <returns>The updated application builder with Swagger middleware configured.</returns>
        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
        {
            // Enable the middleware to serve the generated Swagger as JSON.
            app.UseSwagger();

            // Enable the middleware to serve Swagger UI, specifying the Swagger JSON endpoint and route prefix.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minesweeper API v1");
                c.RoutePrefix = "docs";
            });

            return app;
        }

        /// <summary>
        /// An operation filter that forces the usage of "application/json" media type for both request bodies and responses.
        /// </summary>
        private class ForceJsonOperationFilter : IOperationFilter
        {
            /// <summary>
            /// Applies the filter to ensure that only JSON content types are specified in Swagger documentation.
            /// </summary>
            /// <param name="operation">The OpenAPI operation to modify.</param>
            /// <param name="context">The context of the operation filter.</param>
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                // Force JSON media type for the request body.
                if (operation.RequestBody?.Content != null)
                {
                    // Attempt to find an entry for "application/json" ignoring case.
                    var jsonContent = operation.RequestBody.Content
                        .FirstOrDefault(c => c.Key.Equals("application/json", StringComparison.OrdinalIgnoreCase));
                    if (jsonContent.Value != null)
                    {
                        // Replace the content dictionary with only the JSON media type.
                        operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>
                        {
                            { "application/json", jsonContent.Value }
                        };
                    }
                }

                // Force JSON media type for each response in the operation.
                foreach (var response in operation.Responses)
                {
                    if (response.Value.Content != null)
                    {
                        // Attempt to find an entry for "application/json" ignoring case.
                        var jsonContent = response.Value.Content
                            .FirstOrDefault(c => c.Key.Equals("application/json", StringComparison.OrdinalIgnoreCase));
                        if (jsonContent.Value != null)
                        {
                            // Replace the content dictionary with only the JSON media type.
                            response.Value.Content = new Dictionary<string, OpenApiMediaType>
                            {
                                { "application/json", jsonContent.Value }
                            };
                        }
                    }
                }
            }
        }
    }
}
