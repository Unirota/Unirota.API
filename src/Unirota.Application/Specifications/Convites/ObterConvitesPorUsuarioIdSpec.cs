using Ardalis.Specification;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Specifications.Convites;

public class ObterConvitesPorUsuarioIdSpec : Specification<Convite>
{
    public ObterConvitesPorUsuarioIdSpec(int usuarioId)
    {
        Query.Include(x => x.Grupo)
                .ThenInclude(x => x.Passageiros)
            .Include(x => x.Grupo.Mensagens)
            .Include(x => x.Motorista)
            .Where(x => x.UsuarioId == usuarioId);
    }
}
