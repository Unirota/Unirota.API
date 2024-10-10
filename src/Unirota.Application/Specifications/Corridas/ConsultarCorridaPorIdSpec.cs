using Ardalis.Specification;
using Unirota.Domain.Entities.Corridas;

namespace Unirota.Application.Specification.Corridas;
public class ConsultarCorridaPorIdSpec : Specification<Corrida>
{

    public ConsultarCorridaPorIdSpec(int id)
    {
        Query.Where(corrida => corrida.Id == id);
    }

}