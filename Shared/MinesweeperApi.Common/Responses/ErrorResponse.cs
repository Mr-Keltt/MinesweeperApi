namespace MinesweeperApi.Common.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an error response containing an error code, message, and optional field-specific error details.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the general error code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the descriptive error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the collection of field-specific error details.
        /// </summary>
        public IEnumerable<ErrorResponseFieldInfo> FieldErrors { get; set; }
    }

    /// <summary>
    /// Represents detailed information about an error associated with a specific field.
    /// </summary>
    public class ErrorResponseFieldInfo
    {
        /// <summary>
        /// Gets or sets the error code for the specific field.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the field where the error occurred.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the error message describing the field-specific error.
        /// </summary>
        public string Message { get; set; }
    }
}
