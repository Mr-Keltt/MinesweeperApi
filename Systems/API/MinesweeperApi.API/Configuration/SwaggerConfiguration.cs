using Microsoft.OpenApi.Models;
using MinesweeperApi.Application.Services.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesweeperApi.API.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddAppSwagger(this IServiceCollection services, SwaggerSettings swaggerSettings)
        {
            if (!swaggerSettings.Enabled)
                return services;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minesweeper API",
                    Version = "v1",
                    Description = "API для игры Minesweeper",
                    Contact = new OpenApiContact
                    {
                        Name = "Mr-Keltt",
                        Email = "dmitriydavidenkokeltt@gmail.com",
                        Url = new Uri("https://t.me/MrKelttPro")
                    }
                });

                c.OperationFilter<ForceJsonOperationFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minesweeper API v1");
                c.RoutePrefix = "docs";
            });

            return app;
        }

        private class ForceJsonOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.RequestBody?.Content != null)
                {
                    var jsonContent = operation.RequestBody.Content
                        .FirstOrDefault(c => c.Key.Equals("application/json", StringComparison.OrdinalIgnoreCase));
                    if (jsonContent.Value != null)
                    {
                        operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>
                        {
                            { "application/json", jsonContent.Value }
                        };
                    }
                }

                foreach (var response in operation.Responses)
                {
                    if (response.Value.Content != null)
                    {
                        var jsonContent = response.Value.Content
                            .FirstOrDefault(c => c.Key.Equals("application/json", StringComparison.OrdinalIgnoreCase));
                        if (jsonContent.Value != null)
                        {
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
