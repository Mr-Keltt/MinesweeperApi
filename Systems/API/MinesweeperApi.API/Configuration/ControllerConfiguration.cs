namespace MinesweeperApi.API.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using MinesweeperApi.Common.Extensions;

public static class ControllerConfiguration
{
    public static IServiceCollection AddAppController(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.SetDefaultSettings())
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                    new BadRequestObjectResult(context.ModelState.ToErrorResponse());
            });

        return services;
    }

    public static IEndpointRouteBuilder UseAppController(this IEndpointRouteBuilder app)
    {
        app.MapControllers();

        return app;
    }
}
