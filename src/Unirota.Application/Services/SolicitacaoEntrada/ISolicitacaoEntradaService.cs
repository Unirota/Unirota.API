using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.SolicitacaoEntrada;

public interface ISolicitacaoEntradaService : IScopedService
{
    Task<bool> CriarSolicitacaoEntrada(int usuarioId, int grupoId);
    Task<bool> AceitarSolicitacaoEntrada(int solicitacaoId, CancellationToken cancellationToken);
}