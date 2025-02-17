namespace MinesweeperApi.API.Configuration
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements a self-health check to verify that the API is running correctly.
    /// This health check loads the API assembly and returns its version information as part of the health report.
    /// </summary>
    public class SelfHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Checks the health of the API by loading the API assembly and retrieving its version number.
        /// </summary>
        /// <param name="context">The health check context.</param>
        /// <param name="cancellationToken">
        /// A token that can be used to cancel the health check.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a 
        /// <see cref="HealthCheckResult"/> indicating a healthy status along with the build version of the API.
        /// </returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Load the API assembly to retrieve version information.
            var assembly = Assembly.Load("MinesweeperApi.Api");
            var versionNumber = assembly.GetName().Version;

            // Return a healthy result with the build version included in the description.
            return Task.FromResult(HealthCheckResult.Healthy($"Build {versionNumber}"));
        }
    }
}
