using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.SolicitacaoEntrada;

public interface ISolicitacaoEntradaService : IScopedService
{
    public Task<bool> CriarSolicitacaoEntrada(int usuarioId, int grupoId);
}