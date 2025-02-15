using Microsoft.OpenApi.Models;

namespace MinesweeperApi.API.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {
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
}
