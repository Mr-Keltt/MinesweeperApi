namespace MinesweeperApi.API.Configuration
{
    using Microsoft.Extensions.DependencyInjection;
    using MinesweeperApi.Common.Helpers;

    /// <summary>
    /// Provides extension methods for configuring AutoMapper within the application.
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Registers AutoMapper profiles and related configurations using the AutoMappersRegisterHelper.
        /// </summary>
        /// <param name="services">The service collection to which AutoMapper configurations will be added.</param>
        /// <returns>The updated service collection with AutoMapper configurations registered.</returns>
        public static IServiceCollection AddAppAutoMappers(this IServiceCollection services)
        {
            // Register AutoMapper profiles and mappings using the helper class.
            AutoMappersRegisterHelper.Register(services);

            return services;
        }
    }
}
