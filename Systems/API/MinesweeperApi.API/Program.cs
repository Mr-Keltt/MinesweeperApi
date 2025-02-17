using MinesweeperApi.API;
using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;
using Microsoft.AspNetCore.Builder;

// Load the main application settings, logging settings, and Swagger settings from configuration.
var mainSettings = MinesweeperApi.Common.Settings.Settings.Load<MainSettings>("Main");
var logSettings = MinesweeperApi.Common.Settings.Settings.Load<LogSettings>("Log");
var swaggerSettings = MinesweeperApi.Common.Settings.Settings.Load<SwaggerSettings>("Swagger");

// Create the WebApplication builder.
var builder = WebApplication.CreateBuilder(args);

// Configure the application logger using the main and log settings.
builder.AddAppLogger(mainSettings, logSettings);

// Retrieve the IServiceCollection from the builder.
var services = builder.Services;

// Register application controllers with custom JSON settings and API behavior.
services.AddAppController();

// Register HttpContextAccessor to allow access to HTTP context in services.
services.AddHttpContextAccessor();

// Register application health checks.
services.AddAppHealthChecks();

// Configure Swagger services if enabled via Swagger settings.
services.AddAppSwagger(swaggerSettings);

// Register various application services, repositories, AutoMapper profiles, and settings.
services.RegisterServicesAndModels();

// Register Cross-Origin Resource Sharing (CORS) services.
services.AddAppCors();

// Build the WebApplication.
var app = builder.Build();

// Configure the middleware pipeline.

// Enable CORS middleware.
app.UseAppCors();

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Map controller endpoints (routes) to handle incoming HTTP requests.
app.UseAppController();
app.MapControllers();

// Configure health check endpoints.
app.UseAppHealthChecks();

// Configure Swagger middleware for API documentation if enabled.
app.UseAppSwagger();

// Start the application.
app.Run();
