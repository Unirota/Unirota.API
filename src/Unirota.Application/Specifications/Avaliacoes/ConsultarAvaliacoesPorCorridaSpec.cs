using Ardalis.Specification;
using Unirota.Domain.Entities.Avaliacoes;

namespace Unirota.Application.Specifications.Avaliacoes;

public class ConsultarAvaliacoesPorCorridaSpec : Specification<Avaliacao>
{
    public ConsultarAvaliacoesPorCorridaSpec(int corridaId)
    {
        Query.Where(a => a.CorridaId == corridaId)
            .OrderByDescending(a => a.CreatedAt);
    }
}