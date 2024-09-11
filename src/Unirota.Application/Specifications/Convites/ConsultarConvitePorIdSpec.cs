using Ardalis.Specification;

using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Specifications.Convites;

public class ConsultarConvitePorIdSpec : Specification<Convite>
{
    public ConsultarConvitePorIdSpec(int id)
    {
        Query.Where(usuario => usuario.Id == id);
    }
}
