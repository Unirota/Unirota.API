using Ardalis.Specification;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Specifications.Mensagens;

public class ListarMensagemBaseSpec : Specification<Mensagem>
{
    public ListarMensagemBaseSpec(int grupoId)
    {
        Query
            .Where(grupo => grupo.GrupoId == grupoId)
            .Include(x => x.Usuario)
            .OrderByDescending(grupo => grupo.CreatedAt);
    }
}
