using Mapster;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Mensagens;

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
            .Map(dest => dest.HoraInicio, src => src.HoraInicio)
            .Map(dest => dest.Motorista, src => src.Motorista.Nome)
            .Map(dest => dest.UltimaMensagem, src => src.Mensagens.Last().Conteudo)
            .Map(dest => dest.Destino, src => src.Destino);

        TypeAdapterConfig<Mensagem, ListarMensagensViewModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Conteudo, src => src.Conteudo)
            .Map(dest => dest.UsuarioId, src => src.UsuarioId)
            .Map(dest => dest.NomeUsuario, src => src.Usuario.Nome);
    }
}
