using Mapster;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Services.Corrida;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Avaliacoes;
using Unirota.Application.ViewModels.Avaliacoes;
using Unirota.Domain.Entities.Avaliacoes;

namespace Unirota.Application.Services.Avaliacoes;

public class AvaliacaoService(IRepository<Avaliacao> avaliacaoRepository,
                              ICorridaService corridaService,
                              IUsuarioService usuarioService,
                              IServiceContext serviceContext)
                              : IAvaliacaoService
{
    public async Task<int> Criar(CriarAvaliacaoCommand command, int usuarioId)
    {
        var corridas = await corridaService.ObterPorIdDeGrupo(new ConsultarCorridaPorIdQuery { Id = command.CorridaId }, CancellationToken.None);
        
        if (corridas == null || !corridas.Any() || !corridas.Any(c => c?.Id == command.CorridaId))
        {
            serviceContext.AddError("Corrida não encontrada");
            return 0;
        }
    
        if (!await usuarioService.VerificarUsuarioExiste(usuarioId))
        {
            serviceContext.AddError("Usuário não encontrado");
            return 0;
        }
    
        try
        {
            var avaliacao = new Avaliacao(command.Nota, usuarioId, command.CorridaId);
            await avaliacaoRepository.AddAsync(avaliacao);
            return avaliacao.Id;
        }
        catch (ArgumentException ex)
        {
            serviceContext.AddError(ex.Message);
            return 0;
        }
    }

    public async Task<ICollection<ListarAvaliacoesViewModel>> ObterPorCorridaId(int corridaId)
    {
        var avaliacoes = await avaliacaoRepository.ListAsync(new ConsultarAvaliacoesPorCorridaSpec(corridaId));
        return avaliacoes.Adapt<List<ListarAvaliacoesViewModel>>();
    }
}