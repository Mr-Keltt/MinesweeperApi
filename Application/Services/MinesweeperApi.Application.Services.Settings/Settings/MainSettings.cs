namespace MinesweeperApi.Application.Services.Settings
{
    /// <summary>
    /// Represents the main application settings including public and internal URLs and allowed origins.
    /// </summary>
    public class MainSettings
    {
        /// <summary>
        /// Gets the public URL of the application.
        /// </summary>
        public string PublicUrl { get; private set; }

        /// <summary>
        /// Gets the internal URL of the application.
        /// </summary>
        public string InternalUrl { get; private set; }

        /// <summary>
        /// Gets the allowed origins for cross-origin resource sharing (CORS).
        /// </summary>
        public string AllowedOrigins { get; private set; }
    }
}
