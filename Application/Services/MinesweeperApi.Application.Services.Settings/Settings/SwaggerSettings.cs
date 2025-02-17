namespace MinesweeperApi.Application.Services.Settings
{
    /// <summary>
    /// Represents the configuration settings for Swagger integration.
    /// </summary>
    public class SwaggerSettings
    {
        /// <summary>
        /// Gets a value indicating whether Swagger is enabled.
        /// </summary>
        public bool Enabled { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerSettings"/> class.
        /// Sets Swagger to be disabled by default.
        /// </summary>
        public SwaggerSettings()
        {
            Enabled = false;
        }
    }
}
