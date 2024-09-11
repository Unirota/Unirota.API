using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Validations.Grupos;
using Unirota.Application.Validations.Usuarios;

namespace Unirota.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IUsuarioService, UsuarioService>();

        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidations()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }

    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CriarUsuarioCommand>, CriarUsuarioValidation>();
        services.AddScoped<IValidator<CriarGrupoCommand>, CriarGrupoValidation>();
        return services;
    }
}
