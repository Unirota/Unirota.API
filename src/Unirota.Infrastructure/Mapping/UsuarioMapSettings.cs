using Mapster;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Infrastructure.Mapping;

public static class UsuarioMapSettings
{
    public static void Configure()
    {
        TypeAdapterConfig<Usuario, UsuarioViewModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Motorista, src => !string.IsNullOrEmpty(src.Habilitacao))
            .Map(dest => dest.DataNascimento, src => src.DataNascimento);

    }
}
