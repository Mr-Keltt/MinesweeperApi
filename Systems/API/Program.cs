using MinesweeperApi.API.Configuration;
using MinesweeperApi.API;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddHttpContextAccessor();

services.RegisterServices();


var app = builder.Build();


app.UseAppSwagger();


app.Run();
