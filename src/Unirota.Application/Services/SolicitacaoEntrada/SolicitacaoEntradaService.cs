using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Specifications.SolicitacaoEntrada;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Application.Services.SolicitacaoEntrada;

public class SolicitacaoEntradaService : ISolicitacaoEntradaService

{
    private readonly IRepository<SolicitacaoDeEntrada> _solicitacaoRepository;
    private readonly IServiceContext _serviceContext;
    private readonly ICurrentUser _currentUser;

    public SolicitacaoEntradaService(IRepository<SolicitacaoDeEntrada> solicitacaoRepository, IServiceContext serviceContext, ICurrentUser currentUser)
    {
        _solicitacaoRepository = solicitacaoRepository;
        _serviceContext = serviceContext;
        _currentUser = currentUser;
    }

    public async Task<bool> CriarSolicitacaoEntrada(int usuarioId, int grupoId)
    {
        var solicitacao = new SolicitacaoDeEntrada(usuarioId, grupoId);

        await _solicitacaoRepository.AddAsync(solicitacao);
        return true;
    }

    public async Task<bool> AceitarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken)
    {
        var solicitacao = await _solicitacaoRepository
                            .FirstOrDefaultAsync(new ConsultarSolicitacaoEntradaPorIdSpec(solicitacaoId), cancellationToken);

        if(solicitacao is null)
        {
            _serviceContext.AddError("Solicitação foi excluída ou aceita.");
            return false;
        }

        if(solicitacao.Grupo.MotoristaId != _currentUser.GetUserId())
        {
            _serviceContext.AddError("Somente o motorista do grupo pode aprovar solicitação de entrada");
            return false;
        }

        solicitacao.Aceitar();

        solicitacao.Grupo.AdicionarPassageiro(solicitacao.UsuarioId);
        await _solicitacaoRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RecusarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken)
    {
        var solicitacao = await _solicitacaoRepository
                            .FirstOrDefaultAsync(new ConsultarSolicitacaoEntradaPorIdSpec(solicitacaoId), cancellationToken);

        if (solicitacao is null)
        {
            _serviceContext.AddError("Solicitação foi excluída ou aceita.");
            return false;
        }

        if (solicitacao.Grupo.MotoristaId != _currentUser.GetUserId())
        {
            _serviceContext.AddError("Somente o motorista do grupo pode recusar solicitação de entrada");
            return false;
        }

        await _solicitacaoRepository.DeleteAsync(solicitacao, cancellationToken);

        return true;
    }
}