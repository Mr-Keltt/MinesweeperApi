namespace MinesweeperApi.Application.Services.Logger
{
    using System;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// Provides a logging wrapper around the Serilog logger to standardize log message formatting across the MinesweeperApi.
    /// </summary>
    public class AppLogger : IAppLogger
    {
        // The underlying Serilog logger instance used to write log messages.
        private readonly ILogger logger;

        // The system name used as part of the log message prefix.
        private string _systemName = "MinesweeperApi";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogger"/> class with the specified Serilog logger.
        /// </summary>
        /// <param name="logger">The Serilog logger instance.</param>
        public AppLogger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Constructs a standardized message template by prepending the system name and optional module information.
        /// </summary>
        /// <param name="messageTemplate">The original message template.</param>
        /// <param name="module">Optional module object to include in the template.</param>
        /// <returns>A formatted message template string.</returns>
        private string consructMessageTemplate(string messageTemplate, object module = null)
        {
            var moduleName = module?.GetType().Name;
            if (string.IsNullOrEmpty(moduleName))
                return $"[{_systemName}] {messageTemplate} ";
            else
                return $"[{_systemName}:{module}] {messageTemplate} ";
        }

        /// <inheritdoc/>
        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Write(LogEventLevel level, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, exception, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Write(LogEventLevel level, Exception exception, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Write(level, exception, consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            logger?.Verbose(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Verbose(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Verbose(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            logger?.Debug(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Debug(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Debug(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Information(string messageTemplate, params object[] propertyValues)
        {
            logger?.Information(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Information(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Information(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            logger?.Warning(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Warning(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Warning(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Error(string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Error(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(exception, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Error(Exception exception, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Error(exception, consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Fatal(object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(consructMessageTemplate(messageTemplate, module), propertyValues);
        }

        /// <inheritdoc/>
        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(exception, consructMessageTemplate(messageTemplate), propertyValues);
        }

        /// <inheritdoc/>
        public void Fatal(Exception exception, object module, string messageTemplate, params object[] propertyValues)
        {
            logger?.Fatal(exception, consructMessageTemplate(messageTemplate, module), propertyValues);
        }
    }
}
