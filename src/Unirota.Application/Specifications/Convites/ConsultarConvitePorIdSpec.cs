using Ardalis.Specification;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Specifications.Convites;

public class ConsultarConvitePorId : Specification<Convite>
{
    public ConsultarConvitePorId(int id)
    {
        Query.Where(convite => convite.Id == id);
    }
}