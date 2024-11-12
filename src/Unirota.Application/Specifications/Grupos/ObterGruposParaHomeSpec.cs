using Ardalis.Specification;
using Unirota.Application.Queries.Grupo;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Specifications.Grupos;

public class ObterGruposParaHomeSpec : Specification<Grupo>
{
    public ObterGruposParaHomeSpec(ObterGruposHomeQuery request)
    {
        Query.Include(x => x.Motorista)
             .Include(x => x.Corridas)
                .ThenInclude(x => x.Avaliacoes);

        if(!string.IsNullOrEmpty(request.Destino))
        {
            Query.Where(x => x.Destino.Contains(request.Destino));
        }

        if(request.HoraInicio.HasValue)
        {
            Query.Where(x => x.HoraInicio.TimeOfDay <= request.HoraInicio.Value);
        }
    }
}
