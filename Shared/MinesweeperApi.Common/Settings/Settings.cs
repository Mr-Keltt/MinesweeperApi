namespace MinesweeperApi.Common.Settings
{
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Provides a base class for loading configuration settings.
    /// </summary>
    public abstract class Settings
    {
        /// <summary>
        /// Loads an instance of the specified settings type from the configuration.
        /// </summary>
        /// <typeparam name="T">The type of settings to load.</typeparam>
        /// <param name="key">The configuration section key for the settings.</param>
        /// <param name="configuration">
        /// An optional <see cref="IConfiguration"/> instance. If not provided, the default configuration is used.
        /// </param>
        /// <returns>An instance of type <typeparamref name="T"/> populated with configuration values.</returns>
        public static T Load<T>(string key, IConfiguration configuration = null)
        {
            // Create an instance of the settings type.
            var settings = (T)Activator.CreateInstance(typeof(T));

            // Retrieve a configuration section using the provided key and bind it to the settings instance.
            // Bind non-public properties as well.
            SettingsFactory.Create(configuration)
                .GetSection(key)
                .Bind(settings, options => { options.BindNonPublicProperties = true; });

            return settings;
        }
    }
}
