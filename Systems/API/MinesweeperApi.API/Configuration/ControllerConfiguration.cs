namespace MinesweeperApi.API.Configuration
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
    using MinesweeperApi.Common.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides extension methods for configuring controllers and API behavior in the MinesweeperApi application.
    /// </summary>
    public static class ControllerConfiguration
    {
        /// <summary>
        /// Adds and configures application controllers with Newtonsoft.Json settings and custom API behavior.
        /// </summary>
        /// <param name="services">The service collection to which controller services will be added.</param>
        /// <returns>The updated service collection with configured controllers.</returns>
        public static IServiceCollection AddAppController(this IServiceCollection services)
        {
            services
                // Adds controller support to the application.
                .AddControllers()
                // Configures Newtonsoft.Json to use the default settings defined in the extension method.
                .AddNewtonsoftJson(options => options.SerializerSettings.SetDefaultSettings())
                // Configures custom API behavior for invalid model state responses.
                .ConfigureApiBehaviorOptions(options =>
                {
                    // When the model state is invalid, create a BadRequestObjectResult with error details.
                    options.InvalidModelStateResponseFactory = context =>
                        new BadRequestObjectResult(context.ModelState.ToErrorResponse());
                });

            return services;
        }

        /// <summary>
        /// Maps controller endpoints to the application's endpoint routing.
        /// </summary>
        /// <param name="app">The endpoint route builder used to map endpoints.</param>
        /// <returns>The updated endpoint route builder with controller endpoints mapped.</returns>
        public static IEndpointRouteBuilder UseAppController(this IEndpointRouteBuilder app)
        {
            // Maps controller routes so that incoming HTTP requests are dispatched to the correct controllers.
            app.MapControllers();

            return app;
        }
    }
}
