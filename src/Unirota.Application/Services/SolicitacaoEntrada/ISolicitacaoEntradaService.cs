using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.SolicitacaoEntrada;

public interface ISolicitacaoEntradaService : IScopedService
{
    Task<bool> CriarSolicitacaoEntrada(int usuarioId, int grupoId);
    Task<bool> AceitarSolicitacaoEntrada(int solicitacaoId, string contextId, CancellationToken cancellationToken);
    Task<bool> RecusarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken);
    Task<bool> CancelarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken);
}