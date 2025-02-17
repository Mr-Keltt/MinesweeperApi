namespace MinesweeperApi.API.Configuration
{
    using System;
    using Serilog;
    using Serilog.Events;
    using Microsoft.AspNetCore.Builder;
    using MinesweeperApi.Application.Services.Settings;

    /// <summary>
    /// Provides methods for configuring the application logger using Serilog.
    /// </summary>
    public static class LoggerConfiguration
    {
        /// <summary>
        /// Configures and adds the application logger to the WebApplicationBuilder using the specified main and log settings.
        /// </summary>
        /// <param name="builder">The WebApplicationBuilder to which the logger will be applied.</param>
        /// <param name="mainSettings">The main application settings.</param>
        /// <param name="logSettings">The logging configuration settings.</param>
        public static void AddAppLogger(this WebApplicationBuilder builder, MainSettings mainSettings, LogSettings logSettings)
        {
            // Create a new Serilog logger configuration instance.
            var loggerConfiguration = new Serilog.LoggerConfiguration();

            // Base configuration: enrich log events with a correlation ID header and log context information.
            loggerConfiguration
                .Enrich.WithCorrelationIdHeader()
                .Enrich.FromLogContext();

            // Parse the log level from the settings; default to Information if parsing fails.
            if (!Enum.TryParse(logSettings.Level, out LogLevel level))
                level = LogLevel.Information;

            // Map the custom LogLevel to the corresponding Serilog LogEventLevel.
            var serilogLevel = level switch
            {
                LogLevel.Verbose => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Fatal => LogEventLevel.Fatal,
                _ => LogEventLevel.Information
            };

            // Set the minimum logging levels for the application and override levels for specific namespaces.
            loggerConfiguration
                .MinimumLevel.Is(serilogLevel)
                .MinimumLevel.Override("Microsoft", serilogLevel)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", serilogLevel)
                .MinimumLevel.Override("System", serilogLevel);

            // Define the log output template.
            var logItemTemplate = "[{Timestamp:HH:mm:ss:fff} {Level:u3} ({CorrelationId})] {Message:lj}{NewLine}{Exception}";

            // Configure console logging if enabled.
            if (logSettings.WriteToConsole)
            {
                loggerConfiguration.WriteTo.Console(
                    serilogLevel,
                    logItemTemplate
                );
            }

            // Configure file logging if enabled.
            if (logSettings.WriteToFile)
            {
                // Parse the file rolling interval from settings; default to Day if parsing fails.
                if (!Enum.TryParse(logSettings.FileRollingInterval, out LogRollingInterval interval))
                    interval = LogRollingInterval.Day;

                // Map the custom LogRollingInterval to the corresponding Serilog RollingInterval.
                var serilogInterval = interval switch
                {
                    LogRollingInterval.Infinite => RollingInterval.Infinite,
                    LogRollingInterval.Year => RollingInterval.Year,
                    LogRollingInterval.Month => RollingInterval.Month,
                    LogRollingInterval.Day => RollingInterval.Day,
                    LogRollingInterval.Hour => RollingInterval.Hour,
                    LogRollingInterval.Minute => RollingInterval.Minute,
                    _ => RollingInterval.Day
                };

                // Parse the file size limit; default to 5 MB if parsing fails.
                if (!int.TryParse(logSettings.FileRollingSize, out var size))
                    size = 5242880;

                // Define the log file name.
                var fileName = $"_.log";

                // Configure file sink with rolling file parameters.
                loggerConfiguration.WriteTo.File(
                    $"logs/{fileName}",
                    serilogLevel,
                    logItemTemplate,
                    rollingInterval: serilogInterval,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: size
                );
            }

            // Create the logger instance.
            var logger = loggerConfiguration.CreateLogger();

            // Apply the configured logger to the application builder.
            builder.Host.UseSerilog(logger, true);
        }
    }
}
