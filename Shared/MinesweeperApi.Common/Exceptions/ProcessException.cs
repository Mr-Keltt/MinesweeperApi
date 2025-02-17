using Microsoft.AspNetCore.Identity;
using System;

namespace MinesweeperApi.Common.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs during a process, encapsulating additional error information such as an error code and name.
    /// </summary>
    public class ProcessException : Exception
    {
        /// <summary>
        /// Gets the error code associated with the process exception.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the name associated with the process exception.
        /// </summary>
        public string Name { get; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class.
        /// </summary>
        public ProcessException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProcessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a specified inner exception.
        /// </summary>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ProcessException(Exception inner) : base(inner.Message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ProcessException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a specified error code and error message.
        /// </summary>
        /// <param name="code">The error code associated with the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public ProcessException(string code, string message) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a specified error code, error message, and inner exception.
        /// </summary>
        /// <param name="code">The error code associated with the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ProcessException(string code, string message, Exception inner) : base(message, inner)
        {
            Code = code;
        }

        #endregion

        /// <summary>
        /// Throws a <see cref="ProcessException"/> with the specified message if the provided predicate evaluates to true.
        /// </summary>
        /// <param name="predicate">A function that returns a boolean value indicating whether the exception should be thrown.</param>
        /// <param name="message">The error message to include in the exception if thrown.</param>
        public static void ThrowIf(Func<bool> predicate, string message)
        {
            if (predicate.Invoke())
                throw new ProcessException(message);
        }

        /// <summary>
        /// Throws a <see cref="ProcessException"/> if the provided <see cref="IdentityResult"/> is not successful.
        /// </summary>
        /// <param name="result">The <see cref="IdentityResult"/> to check for success.</param>
        /// <exception cref="ProcessException">Thrown when the identity result indicates failure.</exception>
        public static void ThrowIfNotSuccessful(IdentityResult result)
        {
            // This method is intended to throw an exception if the result is not successful.
            // The actual implementation should inspect the result.Errors collection and throw an exception accordingly.
            throw new NotImplementedException();
        }
    }
}
