namespace MinesweeperApi.Common.Extensions
{
    using Exceptions;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Responses;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for converting exceptions and model state errors into a standardized <see cref="ErrorResponse"/>.
    /// </summary>
    public static class ErrorResponseExtensions
    {
        /// <summary>
        /// Converts a <see cref="ProcessException"/> into an <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="data">The <see cref="ProcessException"/> to convert.</param>
        /// <returns>An <see cref="ErrorResponse"/> containing the error code and message.</returns>
        public static ErrorResponse ToErrorResponse(this ProcessException data)
        {
            var code = "";
            var message = "";

            var res = new ErrorResponse()
            {
                Code = data.Code ?? code,
                Message = data.Message ?? message
            };

            return res;
        }

        /// <summary>
        /// Converts a general <see cref="Exception"/> into an <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="data">The <see cref="Exception"/> to convert.</param>
        /// <returns>An <see cref="ErrorResponse"/> containing the exception message.</returns>
        public static ErrorResponse ToErrorResponse(this Exception data)
        {
            var code = "";
            var message = "";

            var res = new ErrorResponse()
            {
                Code = code,
                Message = data.Message ?? message
            };

            return res;
        }

        /// <summary>
        /// Converts a <see cref="ForbidAccessException"/> into an <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="data">The <see cref="ForbidAccessException"/> to convert.</param>
        /// <returns>An <see cref="ErrorResponse"/> containing the error code and message.</returns>
        public static ErrorResponse ToErrorResponse(this ForbidAccessException data)
        {
            var code = "";
            var message = "";

            var res = new ErrorResponse()
            {
                Code = data.Code ?? code,
                Message = data.Message ?? message
            };

            return res;
        }

        /// <summary>
        /// Converts a <see cref="ValidationException"/> from FluentValidation into an <see cref="ErrorResponse"/>.
        /// </summary>
        /// <param name="data">The <see cref="ValidationException"/> to convert.</param>
        /// <returns>An <see cref="ErrorResponse"/> containing validation error details.</returns>
        public static ErrorResponse ToErrorResponse(this ValidationException data)
        {
            var code = "";
            var message = "";

            var res = new ErrorResponse()
            {
                Code = code,
                Message = message,
                FieldErrors = data.Errors.Select(e => new ErrorResponseFieldInfo()
                {
                    Code = e.ErrorCode,
                    FieldName = e.PropertyName,
                    Message = e.ErrorMessage
                })
            };

            return res;
        }

        /// <summary>
        /// Converts a <see cref="ModelStateDictionary"/> into an <see cref="ErrorResponse"/>, aggregating all field errors.
        /// </summary>
        /// <param name="data">The <see cref="ModelStateDictionary"/> containing validation errors.</param>
        /// <returns>An <see cref="ErrorResponse"/> with error details for each invalid field.</returns>
        public static ErrorResponse ToErrorResponse(this ModelStateDictionary data)
        {
            var fieldErrors = new List<ErrorResponseFieldInfo>();

            foreach (var (field, state) in data)
            {
                if (state.ValidationState == ModelValidationState.Invalid)
                {
                    fieldErrors.Add(new ErrorResponseFieldInfo()
                    {
                        Code = "",
                        FieldName = field.ToCamelCase(),
                        Message = string.Join(", ", state.Errors.Select(x => x.ErrorMessage))
                    });
                }
            }

            var res = new ErrorResponse()
            {
                Code = "",
                Message = "",
                FieldErrors = fieldErrors
            };

            return res;
        }
    }
}
