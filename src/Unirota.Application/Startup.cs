using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Common;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Validations.Avaliacoes;
using Unirota.Application.Validations.Grupos;
using Unirota.Application.Validations.Mensagens;
using Unirota.Application.Validations.Usuarios;
using Unirota.Application.Validations.Veiculos;

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
        services.AddScoped<IValidator<CriarMensagemCommand>, CriarMensagemValidation>();
        services.AddScoped<IValidator<CriarVeiculosCommand>, CriarVeiculoValidation>();
        services.AddScoped<IValidator<CriarAvaliacaoCommand>, CriarAvaliacaoValidation>();
        return services;
    }
}
