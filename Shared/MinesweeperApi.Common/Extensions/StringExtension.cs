namespace MinesweeperApi.Common.Extensions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.Json;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides extension methods for <see cref="string"/> manipulation and formatting.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Determines whether the specified string is null or an empty string.
        /// </summary>
        /// <param name="data">The string to check.</param>
        /// <returns>
        /// <c>true</c> if the string is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string data)
        {
            return string.IsNullOrEmpty(data);
        }

        /// <summary>
        /// Determines whether the specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="data">The string to check.</param>
        /// <returns>
        /// <c>true</c> if the string is null, empty, or white-space; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string data)
        {
            return string.IsNullOrWhiteSpace(data);
        }

        /// <summary>
        /// Converts the specified text to title case (each word capitalized).
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>
        /// The text converted to title case, or <c>null</c> if the input text is <c>null</c>.
        /// </returns>
        public static string ToTitleCase(this string text)
        {
            if (text == null)
                return null;

            // Create a TextInfo based on the "en-US" culture.
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            // Convert the text to lower case and then to title case.
            text = textInfo.ToTitleCase(text.ToLower());
            return text;
        }

        /// <summary>
        /// Converts the specified string to camelCase using the default JSON naming policy.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The string converted to camelCase.</returns>
        public static string ToCamelCase(this string str)
        {
            return JsonNamingPolicy.CamelCase.ConvertName(str);
        }

        /// <summary>
        /// Splits the string into chunks of a specified size.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing string chunks of the specified size. If the string is null or empty, returns an empty list.
        /// </returns>
        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string>();

            if (str.Length < chunkSize)
                return new List<string> { str };

            // Calculate the number of chunks and return the substrings.
            return Enumerable.Range(0, str.Length / chunkSize)
                             .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        /// <summary>
        /// Removes extra white-space characters from the string, leaving only single spaces between words.
        /// </summary>
        /// <param name="str">The string to process.</param>
        /// <returns>
        /// A string with all sequences of white-space characters replaced by a single space.
        /// </returns>
        public static string SquashWhiteSpaces(this string str)
        {
            str = str.Trim();
            // Use a regular expression to replace multiple white-space characters with a single space.
            return Regex.Replace(str, @"\s+", " ");
        }
    }
}
