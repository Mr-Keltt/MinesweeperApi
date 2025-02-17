namespace MinesweeperApi.Common.Exceptions
{
    using System;

    /// <summary>
    /// Represents an exception that is thrown when access is forbidden.
    /// This exception includes an optional error code for additional context.
    /// </summary>
    public class ForbidAccessException : Exception
    {
        /// <summary>
        /// Gets the error code associated with the forbidden access.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbidAccessException"/> class.
        /// </summary>
        public ForbidAccessException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbidAccessException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ForbidAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbidAccessException"/> class with a specified inner exception.
        /// </summary>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ForbidAccessException(Exception inner) : base(inner.Message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbidAccessException"/> class with a specified 
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ForbidAccessException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbidAccessException"/> class with a specified 
        /// error code and error message.
        /// </summary>
        /// <param name="code">The error code associated with the forbidden access.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ForbidAccessException(string code, string message) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbidAccessException"/> class with a specified 
        /// error code, error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="code">The error code associated with the forbidden access.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ForbidAccessException(string code, string message, Exception inner) : base(message, inner)
        {
            Code = code;
        }
    }
}
