namespace MinesweeperApi.Application.Services.Logger
{
    using System;
    using Serilog.Events;

    /// <summary>
    /// Defines a logging interface for writing log messages with different severity levels and contextual information.
    /// </summary>
    public interface IAppLogger
    {
        /// <summary>
        /// Writes a log message with the specified severity level.
        /// </summary>
        /// <param name="level">The severity level of the log event.</param>
        /// <param name="messageTemplate">The message template for the log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a log message with the specified severity level and module context.
        /// </summary>
        /// <param name="level">The severity level of the log event.</param>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Write(LogEventLevel level, object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a log message with an associated exception at the specified severity level.
        /// </summary>
        /// <param name="level">The severity level of the log event.</param>
        /// <param name="exception">The exception related to the log event.</param>
        /// <param name="messageTemplate">The message template for the log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a log message with an associated exception and module context at the specified severity level.
        /// </summary>
        /// <param name="level">The severity level of the log event.</param>
        /// <param name="exception">The exception related to the log event.</param>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Write(LogEventLevel level, Exception exception, object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a verbose-level log message.
        /// </summary>
        /// <param name="messageTemplate">The message template for the verbose log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Verbose(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a verbose-level log message with module context.
        /// </summary>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the verbose log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Verbose(object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a debug-level log message.
        /// </summary>
        /// <param name="messageTemplate">The message template for the debug log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Debug(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a debug-level log message with module context.
        /// </summary>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the debug log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Debug(object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes an informational log message.
        /// </summary>
        /// <param name="messageTemplate">The message template for the informational log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Information(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes an informational log message with module context.
        /// </summary>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the informational log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Information(object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="messageTemplate">The message template for the warning log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Warning(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a warning log message with module context.
        /// </summary>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the warning log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Warning(object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes an error log message.
        /// </summary>
        /// <param name="messageTemplate">The message template for the error log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Error(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes an error log message with module context.
        /// </summary>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the error log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Error(object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes an error log message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception related to the error log event.</param>
        /// <param name="messageTemplate">The message template for the error log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes an error log message with an associated exception and module context.
        /// </summary>
        /// <param name="exception">The exception related to the error log event.</param>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the error log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Error(Exception exception, object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a fatal-level log message.
        /// </summary>
        /// <param name="messageTemplate">The message template for the fatal log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Fatal(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a fatal-level log message with module context.
        /// </summary>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the fatal log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Fatal(object module, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a fatal-level log message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception related to the fatal log event.</param>
        /// <param name="messageTemplate">The message template for the fatal log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Writes a fatal-level log message with an associated exception and module context.
        /// </summary>
        /// <param name="exception">The exception related to the fatal log event.</param>
        /// <param name="module">The module context associated with the log event.</param>
        /// <param name="messageTemplate">The message template for the fatal log event.</param>
        /// <param name="propertyValues">The property values to format the message template.</param>
        void Fatal(Exception exception, object module, string messageTemplate, params object[] propertyValues);
    }
}
