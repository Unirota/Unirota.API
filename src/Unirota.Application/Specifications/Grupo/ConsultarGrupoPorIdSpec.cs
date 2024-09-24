using Ardalis.Specification;

namespace Unirota.Application.Specifications.Grupo;

public class ConsultarGrupoPorIdSpec : Specification<Domain.Entities.Grupos.Grupo>
{
    public ConsultarGrupoPorIdSpec(int id)
    {
        Query.Where(grupo => grupo.Id.Equals(id));
    }

}
