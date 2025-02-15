#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/API/MinesweeperApi.API/MinesweeperApi.API.csproj", "Systems/API/MinesweeperApi.API/"]
COPY ["Application/Models/MinesweeperApi.Application.Models.CommonModels/MinesweeperApi.Application.Models.CommonModels.csproj", "Application/Models/MinesweeperApi.Application.Models.CommonModels/"]
COPY ["Application/Services/MinesweeperApi.Application.Services.GameController/MinesweeperApi.Application.Services.GameController.csproj", "Application/Services/MinesweeperApi.Application.Services.GameController/"]
COPY ["Application/Services/MinesweeperApi.Application.Services.Logger/MinesweeperApi.Application.Services.Logger.csproj", "Application/Services/MinesweeperApi.Application.Services.Logger/"]
COPY ["Application/Services/MinesweeperApi.Application.Services.Settings/MinesweeperApi.Application.Services.Settings.csproj", "Application/Services/MinesweeperApi.Application.Services.Settings/"]
COPY ["Shared/MinesweeperApi.Common/MinesweeperApi.Common.csproj", "Shared/MinesweeperApi.Common/"]
COPY ["Infrastructure/Data/MinesweeperApi.Infrastructure.Data.Configuration/MinesweeperApi.Infrastructure.Data.Configuration.csproj", "Infrastructure/Data/MinesweeperApi.Infrastructure.Data.Configuration/"]
COPY ["Infrastructure/Data/MinesweeperApi.Infrastructure.Data.Context/MinesweeperApi.Infrastructure.Data.Context.csproj", "Infrastructure/Data/MinesweeperApi.Infrastructure.Data.Context/"]
COPY ["Infrastructure/Data/MinesweeperApi.Infrastructure.Data.Entities/MinesweeperApi.Infrastructure.Data.Entities.csproj", "Infrastructure/Data/MinesweeperApi.Infrastructure.Data.Entities/"]
COPY ["Infrastructure/Repositories/MinesweeperApi.Infrastructure.Repositories.Data/MinesweeperApi.Infrastructure.Repositories.Data.csproj", "Infrastructure/Repositories/MinesweeperApi.Infrastructure.Repositories.Data/"]
RUN dotnet restore "./Systems/API/MinesweeperApi.API/MinesweeperApi.API.csproj"
COPY . .
WORKDIR "/src/Systems/API/MinesweeperApi.API"
RUN dotnet build "./MinesweeperApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MinesweeperApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinesweeperApi.API.dll"]