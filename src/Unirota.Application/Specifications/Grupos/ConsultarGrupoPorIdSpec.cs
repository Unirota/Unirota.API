using Ardalis.Specification;

namespace Unirota.Application.Specifications.Grupos;

public class ConsultarGrupoPorIdSpec : Specification<Domain.Entities.Grupos.Grupo>
{
    public ConsultarGrupoPorIdSpec(int id)
    {
        Query
            .Include(grupo => grupo.Passageiros)
            .Include(grupo => grupo.Motorista)
            .Where(grupo => grupo.Id == id);
    }
}
