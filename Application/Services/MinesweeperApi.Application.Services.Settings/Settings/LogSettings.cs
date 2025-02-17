namespace MinesweeperApi.Application.Services.Settings
{
    /// <summary>
    /// Represents configuration settings for logging within the application.
    /// </summary>
    public class LogSettings
    {
        /// <summary>
        /// Gets the logging level as a string. Expected values correspond to the <see cref="LogLevel"/> enumeration.
        /// </summary>
        public string Level { get; private set; }

        /// <summary>
        /// Gets a value indicating whether logs should be written to the console.
        /// </summary>
        public bool WriteToConsole { get; private set; }

        /// <summary>
        /// Gets a value indicating whether logs should be written to a file.
        /// </summary>
        public bool WriteToFile { get; private set; }

        /// <summary>
        /// Gets the file rolling interval as a string. Expected values correspond to the <see cref="LogRollingInterval"/> enumeration.
        /// </summary>
        public string FileRollingInterval { get; private set; }

        /// <summary>
        /// Gets the file rolling size as a string that defines the maximum size before rolling the log file.
        /// </summary>
        public string FileRollingSize { get; private set; }
    }

    /// <summary>
    /// Specifies the level of logging for operational and diagnostic purposes.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Verbose logging. Contains everything you might want to know about a running block of code.
        /// </summary>
        Verbose,

        /// <summary>
        /// Debug logging. Provides internal system events that are not necessarily observable from the outside.
        /// </summary>
        Debug,

        /// <summary>
        /// Information logging. Represents key operational events in the application.
        /// </summary>
        Information,

        /// <summary>
        /// Warning logging. Indicates that the service is degraded or endangered.
        /// </summary>
        Warning,

        /// <summary>
        /// Error logging. Indicates that functionality is unavailable, invariants are broken, or data is lost.
        /// </summary>
        Error,

        /// <summary>
        /// Fatal logging. Indicates critical failures that require immediate attention.
        /// </summary>
        Fatal
    }

    /// <summary>
    /// Specifies the interval at which log files should roll over.
    /// </summary>
    public enum LogRollingInterval
    {
        /// <summary>
        /// Infinite interval. The log file will never roll; no time period information will be appended to the log file name.
        /// </summary>
        Infinite,

        /// <summary>
        /// Yearly interval. The log file will roll every year, with filenames appended with the four-digit year (yyyy).
        /// </summary>
        Year,

        /// <summary>
        /// Monthly interval. The log file will roll every calendar month, with filenames appended with the year and month (yyyyMM).
        /// </summary>
        Month,

        /// <summary>
        /// Daily interval. The log file will roll every day, with filenames appended with the date (yyyyMMdd).
        /// </summary>
        Day,

        /// <summary>
        /// Hourly interval. The log file will roll every hour, with filenames appended with the date and hour (yyyyMMddHH).
        /// </summary>
        Hour,

        /// <summary>
        /// Minute interval. The log file will roll every minute, with filenames appended with the date, hour, and minute (yyyyMMddHHmm).
        /// </summary>
        Minute
    }
}
