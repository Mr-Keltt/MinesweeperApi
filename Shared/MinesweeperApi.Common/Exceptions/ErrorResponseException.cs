namespace MinesweeperApi.Common.Exceptions
{
    using System;
    using Responses;

    /// <summary>
    /// Represents an exception that encapsulates an <see cref="ErrorResponse"/> object.
    /// This exception is used to propagate detailed error responses throughout the application.
    /// </summary>
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// Gets the <see cref="ErrorResponse"/> associated with this exception.
        /// </summary>
        public ErrorResponse ErrorResponse { get; } = new();

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class.
        /// </summary>
        public ErrorResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ErrorResponseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified inner exception.
        /// </summary>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ErrorResponseException(Exception inner)
            : base(inner.Message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ErrorResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="errorResponse">The error response object containing detailed error information.</param>
        public ErrorResponseException(ErrorResponse errorResponse)
            : base()
        {
            ErrorResponse = errorResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified <see cref="ErrorResponse"/>
        /// and inner exception.
        /// </summary>
        /// <param name="errorResponse">The error response object containing detailed error information.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ErrorResponseException(ErrorResponse errorResponse, Exception inner)
            : base(null, inner)
        {
            ErrorResponse = errorResponse;
        }

        #endregion
    }
}
