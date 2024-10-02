using Ardalis.Specification;

namespace Unirota.Application.Specifications.Grupos;

public class ConsultarGrupoPorIdSpec : Specification<Domain.Entities.Grupos.Grupo>
{
    public ConsultarGrupoPorIdSpec(int id)
    {
        Query
            .Where(grupo => grupo.Id == id);
    }
}
