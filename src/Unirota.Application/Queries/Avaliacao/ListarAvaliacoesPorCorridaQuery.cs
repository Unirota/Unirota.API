using MediatR;
using Unirota.Application.ViewModels.Avaliacoes;

namespace Unirota.Application.Queries.Avaliacao;

public class ListarAvaliacoesPorCorridaQuery : IRequest<ICollection<ListarAvaliacoesViewModel>>
{
    public int CorridaId { get; set; }
}