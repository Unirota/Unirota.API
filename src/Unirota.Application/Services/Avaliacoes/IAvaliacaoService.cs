using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.Avaliacoes;

public interface IAvaliacaoService : IScopedService
{
    Task<int> Criar(CriarAvaliacaoCommand command, int usuarioId);

}