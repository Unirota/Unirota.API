using Ardalis.Specification;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Specifications.Convites;

public class ConsultarConvitePorIdSpec : Specification<Convite>
{
    public ConsultarConvitePorIdSpec(int id)
    {
        Query.Include(x => x.Grupo)
            .Where(convite => convite.Id == id);
    }

    public ConsultarConvitePorIdSpec(int usuarioId, int motoristaId, int grupoId)
    {
        Query.Where(convite =>
            convite.UsuarioId == usuarioId &&
            convite.MotoristaId == motoristaId &&
            convite.GrupoId == grupoId);
    }
}
