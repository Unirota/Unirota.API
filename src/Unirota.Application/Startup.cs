using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Unirota.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidations()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }

    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        return services;
    }
}
