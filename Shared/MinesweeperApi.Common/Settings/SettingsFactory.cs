namespace MinesweeperApi.Common.Settings
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;

    /// <summary>
    /// Provides a factory for creating the application configuration.
    /// </summary>
    public static class SettingsFactory
    {
        /// <summary>
        /// Creates an <see cref="IConfiguration"/> instance.
        /// If a configuration is not provided, it builds one using the application's base directory, JSON configuration files, and environment variables.
        /// </summary>
        /// <param name="configuration">
        /// An optional <see cref="IConfiguration"/> instance. If provided, it will be used; otherwise, a new configuration is built.
        /// </param>
        /// <returns>An <see cref="IConfiguration"/> instance containing the application settings.</returns>
        public static IConfiguration Create(IConfiguration configuration = null)
        {
            // If a configuration instance is provided, return it; otherwise, build a new configuration.
            var conf = configuration ?? new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                // Load the main appsettings.json file (required)
                .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), optional: false)
                // Load the optional appsettings.development.json file for development-specific settings
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.development.json"), optional: true)
                // Add environment variables to the configuration
                .AddEnvironmentVariables()
                .Build();

            return conf;
        }
    }
}
