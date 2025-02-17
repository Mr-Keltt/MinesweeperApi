namespace MinesweeperApi.Common.HealthCheck
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides helper methods for formatting and writing health check responses.
    /// </summary>
    public static class HealthCheckHelper
    {
        /// <summary>
        /// Writes a detailed health check response as a JSON object to the HTTP response.
        /// </summary>
        /// <param name="context">The HTTP context used to write the response.</param>
        /// <param name="report">The health report containing the overall and individual health check statuses.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
        {
            // Set the content type to application/json.
            context.Response.ContentType = "application/json";

            // Create a HealthCheck response object that includes overall status,
            // total duration, and a list of individual health check items.
            var response = new HealthCheck()
            {
                OverallStatus = report.Status.ToString(),
                TotalDuration = report.TotalDuration.TotalSeconds.ToString("0:0.00"),
                HealthChecks = report.Entries.Select(x => new HealthCheckItem
                {
                    Status = x.Value.Status.ToString(),
                    Component = x.Key,
                    Description = x.Value.Description ?? string.Empty,
                    Duration = x.Value.Duration.TotalSeconds.ToString("0:0.00")
                })
            };

            // Serialize the health check response with indented formatting.
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

            // Write the JSON response to the HTTP context.
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
