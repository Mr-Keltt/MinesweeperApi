using MinesweeperApi.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppHealthChecks();

services.AddSwaggerSettings();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseAppHealthChecks();

app.UseAppSwagger();

app.Run();
