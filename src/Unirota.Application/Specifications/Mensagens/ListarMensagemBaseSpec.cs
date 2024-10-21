using Ardalis.Specification;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Specifications.Mensagens;

public class ListarMensagemBaseSpec : Specification<Mensagem>
{
    public ListarMensagemBaseSpec(int pagina,
                                  int quantidadeRegistros,
                                  int grupoId)
    {
        Query
            .Where(grupo => grupo.Id == grupoId)
            .Skip((pagina - 1) * quantidadeRegistros)
            .Take(quantidadeRegistros)
            .Include(x => x.Usuario)
            .OrderByDescending(grupo => grupo.CreatedAt);
    }
}
