namespace MinesweeperApi.API.Configuration;

using MinesweeperApi.Common.Helpers;

public static class AutoMapperConfiguration
{
    public static IServiceCollection AddAppAutoMappers(this IServiceCollection services)
    {
        AutoMappersRegisterHelper.Register(services);

        return services;
    }
}