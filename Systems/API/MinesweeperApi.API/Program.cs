using MinesweeperApi.API;
using MinesweeperApi.API.Configuration;
using MinesweeperApi.Application.Services.Settings;


var mainSettings = MinesweeperApi.Common.Settings.Settings.Load<MainSettings>("Main");
var logSettings = MinesweeperApi.Common.Settings.Settings.Load<LogSettings>("Log");
var swaggerSettings = MinesweeperApi.Common.Settings.Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddAppController();

services.AddHttpContextAccessor();

services.AddAppHealthChecks();

services.AddAppSwagger(swaggerSettings);

services.RegisterServicesAndModels();

services.AddAppCors();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAppCors();

app.UseHttpsRedirection();

app.UseAppController();

app.MapControllers();

app.UseRouting();

app.UseAppHealthChecks();

app.UseAppSwagger();

app.Run();
