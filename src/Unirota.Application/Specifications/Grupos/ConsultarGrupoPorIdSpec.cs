using Ardalis.Specification;

namespace Unirota.Application.Specifications.Grupos;

public class ConsultarGrupoPorIdSpec : Specification<Domain.Entities.Grupos.Grupo>, ISingleResultSpecification
{
    public ConsultarGrupoPorIdSpec(int id)
    {
        Query
            .Where(grupo => grupo.Id == id)
            .AsNoTracking();
    }
}
