using Microsoft.AspNetCore.SignalR;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Hubs;
using Unirota.Application.Persistence;
using Unirota.Application.Specifications.SolicitacaoEntrada;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Application.Services.SolicitacaoEntrada;

public class SolicitacaoEntradaService : ISolicitacaoEntradaService

{
    private readonly IRepository<SolicitacaoDeEntrada> _solicitacaoRepository;
    private readonly IServiceContext _serviceContext;
    private readonly ICurrentUser _currentUser;
    private readonly IHubContext<ChatHub> _chatHub;

    public SolicitacaoEntradaService(IRepository<SolicitacaoDeEntrada> solicitacaoRepository, IServiceContext serviceContext, ICurrentUser currentUser, IHubContext<ChatHub> chatHub)
    {
        _solicitacaoRepository = solicitacaoRepository;
        _serviceContext = serviceContext;
        _currentUser = currentUser;
        _chatHub = chatHub;
    }

    public async Task<bool> CriarSolicitacaoEntrada(int usuarioId, int grupoId)
    {
        var solicitacao = new SolicitacaoDeEntrada(usuarioId, grupoId);

        await _solicitacaoRepository.AddAsync(solicitacao);
        return true;
    }

    public async Task<bool> AceitarSolicitacaoEntrada(int solicitacaoId, string contextId, CancellationToken cancellationToken)
    {
        var solicitacao = await _solicitacaoRepository
                            .FirstOrDefaultAsync(new ConsultarSolicitacaoEntradaPorIdSpec(solicitacaoId), cancellationToken);

        if(solicitacao is null)
        {
            _serviceContext.AddError("Solicita��o foi exclu�da ou aceita.");
            return false;
        }

        if(solicitacao.Grupo.MotoristaId != _currentUser.GetUserId())
        {
            _serviceContext.AddError("Somente o motorista do grupo pode aprovar solicita��o de entrada");
            return false;
        }

        solicitacao.Aceitar();

        solicitacao.Grupo.AdicionarPassageiro(solicitacao.UsuarioId);
        await _solicitacaoRepository.SaveChangesAsync(cancellationToken);
        await _chatHub.Groups.AddToGroupAsync(contextId, solicitacao.Grupo.Id.ToString());
        return true;
    }

    public async Task<bool> RecusarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken)
    {
        var solicitacao = await _solicitacaoRepository
                            .FirstOrDefaultAsync(new ConsultarSolicitacaoEntradaPorIdSpec(solicitacaoId), cancellationToken);

        if (solicitacao is null)
        {
            _serviceContext.AddError("Solicita��o foi exclu�da ou aceita.");
            return false;
        }

        if (solicitacao.Grupo.MotoristaId != _currentUser.GetUserId())
        {
            _serviceContext.AddError("Somente o motorista do grupo pode recusar solicita��o de entrada");
            return false;
        }

        await _solicitacaoRepository.DeleteAsync(solicitacao, cancellationToken);

        return true;
    }

    public async Task<bool> CancelarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken)
    {
        var solicitacao = await _solicitacaoRepository
                                        .FirstOrDefaultAsync(new ConsultarSolicitacaoEntradaPorIdSpec(solicitacaoId), cancellationToken);

        if(solicitacao is null)
        {
            _serviceContext.AddError("Solicita��o foi exclu�da ou aceita.");
            return false;
        }

        if(solicitacao.Usuario.Id != _currentUser.GetUserId())
        {
            _serviceContext.AddError("N�o � poss�vel cancelar solicita��o de outro usu�rio.");
            return false;
        }

        await _solicitacaoRepository.DeleteAsync(solicitacao, cancellationToken);

        return true;
    }
}