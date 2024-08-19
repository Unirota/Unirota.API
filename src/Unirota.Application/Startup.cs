using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Unirota.Application.Requests.Usuarios;
using Unirota.Application.Validations.Usuario;

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
        services.AddScoped<IValidator<CriarUsuarioCommand>, CriarUsuarioValidation>();
        return services;
    }
}
