using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unirota.Infrastructure.Common;
using Unirota.Infrastructure.Health;
using Unirota.Infrastructure.Persistence;

namespace Unirota.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO: Adicionar Auth
        return services
            .AddHealthCheck()
            .AddPersistence(configuration)
            .AddServices();
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().AddCheck<ApplicationHealthCheck>("Unirota.Application").Services;
}
