using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Avaliacoes;

namespace Unirota.Application.Services.Avaliacoes;

public interface IAvaliacaoService : IScopedService
{
    Task<int> Criar(CriarAvaliacaoCommand command, int usuarioId);
    Task<ICollection<ListarAvaliacoesViewModel>> ObterPorCorridaId(int corridaId);
}