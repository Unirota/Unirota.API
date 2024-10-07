using Ardalis.Specification;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Specifications.Convites;

public class ConsultarConvitePorIdSpec : Specification<Convite>
{
    public ConsultarConvitePorIdSpec(int id)
    {
        Query.Where(usuario => usuario.Id == id);
    }

    public ConsultarConvitePorIdSpec(int usuarioId, int motoristaId, int grupoId, bool? aceito = null)
    {
        Query.Where(convite =>
            convite.UsuarioId == usuarioId &&
            convite.MotoristaId == motoristaId &&
            convite.GrupoId == grupoId);

        if (aceito.HasValue)
        {
            Query.Where(convite => convite.Aceito == aceito.Value);
        }
    }
}
