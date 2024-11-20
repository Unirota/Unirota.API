using Ardalis.Specification;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Specifications.Convites;

public class ObterConvitesPorUsuarioIdSpec : Specification<Convite>
{
    public ObterConvitesPorUsuarioIdSpec(int usuarioId)
    {
        Query.Include(x => x.Grupo)
                .ThenInclude(x => x.Mensagens)
            .Include(x => x.Motorista)
            .Where(x => x.UsuarioId == usuarioId);
    }
}
