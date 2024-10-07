using Mapster;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Infrastructure.Mapping;

public static class GrupoMapSettings
{
    public static void Configure()
    {
        TypeAdapterConfig<Grupo, ListarGruposViewModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Descricao, src => src.Descricao)
            .Map(dest => dest.HoraInicio, src => src.HoraInicio);
    }
}
