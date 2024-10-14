using Ardalis.Specification;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Specifications.Mensagens;

public class ConsultarMensagensPorGrupoSpec : Specification<Mensagem>
{
    public ConsultarMensagensPorGrupoSpec(int grupoId)
    {
        Query.Where(m => m.GrupoId == grupoId)
            .OrderBy(m => m.CreatedAt);
    }
}