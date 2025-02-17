using Newtonsoft.Json;

namespace MinesweeperApi.API.Models
{
    /// <summary>
    /// Represents an error response containing details about an error that occurred.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
