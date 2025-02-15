namespace MinesweeperApi.Common.Helpers;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public static class AutoMappersRegisterHelper
{
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.StartsWith("MinesweeperApi."));

        services.AddAutoMapper(assemblies);
    }
}