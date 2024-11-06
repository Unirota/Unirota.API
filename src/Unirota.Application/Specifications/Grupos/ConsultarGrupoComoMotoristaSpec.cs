using Ardalis.Specification;

namespace Unirota.Application.Specifications.Grupos;

public class ConsultarGrupoComoMotoristaSpec : Specification<Domain.Entities.Grupos.Grupo>
{
    public ConsultarGrupoComoMotoristaSpec(int usuarioId)
    {
        Query
            .Where(grupo => grupo.MotoristaId == usuarioId)
            .Include(x => x.Corridas);
    }
}
