using MinesweeperApi.API;
using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;
using MinesweeperApi.Common.Settings;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddAppController();

services.AddHttpContextAccessor();

services.AddAppHealthChecks();

services.AddAppSwagger(swaggerSettings);

services.RegisterServices();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAppController();

app.MapControllers();

app.UseRouting();

app.UseAppHealthChecks();

app.UseAppSwagger();

app.Run();
