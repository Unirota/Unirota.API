using Unirota.Application.Persistence;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Application.Services.SolicitacaoEntrada;

public class SolicitacaoEntradaService: ISolicitacaoEntradaService

{
    private readonly IRepository<SolicitacaoDeEntrada> _solicitacaoRepository;

    public SolicitacaoEntradaService(IRepository<SolicitacaoDeEntrada> solicitacaoRepository)
    {
        _solicitacaoRepository = solicitacaoRepository;
    }
    
    public async Task<bool> CriarSolicitacaoEntrada(int usuarioId, int grupoId)
    {
        var solicitacao = new SolicitacaoDeEntrada
        {
            UsuarioId = usuarioId,
            GrupoId = grupoId,
        };

        await _solicitacaoRepository.AddAsync(solicitacao);
        return true;
    }
}