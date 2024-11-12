using Ardalis.Specification;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Specifications.Grupos;

public class ObterGruposParaHomeSpec : Specification<Grupo>
{
    public ObterGruposParaHomeSpec(string destino)
    {
        Query.Where(x => x.Destino.Contains(destino ?? ""))
            .Include(x => x.Motorista)
            .Take(3);     
    }
}
