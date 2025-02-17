namespace MinesweeperApi.Common.HealthCheck
{
    /// <summary>
    /// Represents the overall health check result for the application,
    /// including the overall status, details for each individual health check, and the total duration.
    /// </summary>
    public class HealthCheck
    {
        /// <summary>
        /// Gets or sets the overall status of the health check.
        /// Possible values might include "Healthy", "Degraded", or "Unhealthy".
        /// </summary>
        public string OverallStatus { get; set; }

        /// <summary>
        /// Gets or sets the collection of individual health check items.
        /// Each item represents the health status of a specific component or service.
        /// </summary>
        public IEnumerable<HealthCheckItem> HealthChecks { get; set; }

        /// <summary>
        /// Gets or sets the total duration taken to perform all the health checks.
        /// This is typically represented as a string, e.g., "00:00:02.345".
        /// </summary>
        public string TotalDuration { get; set; }
    }

    /// <summary>
    /// Represents an individual health check item for a specific component or service.
    /// </summary>
    public class HealthCheckItem
    {
        /// <summary>
        /// Gets or sets the status of the health check item.
        /// This could be values like "Healthy", "Degraded", or "Unhealthy".
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the name or identifier of the component that was checked.
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// Gets or sets a description providing additional details about the health check.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the duration of the individual health check.
        /// This value represents the time taken to perform the check, typically formatted as a string.
        /// </summary>
        public string Duration { get; set; }
    }
}
