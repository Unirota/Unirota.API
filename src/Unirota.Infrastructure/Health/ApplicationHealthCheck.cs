using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Unirota.Infrastructure.Health;

public class ApplicationHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellation = default)
    {
        var check = new HealthCheckResult(HealthStatus.Healthy, "Aplicação executando devidamente.");
        return Task.FromResult(check);
    }
}
