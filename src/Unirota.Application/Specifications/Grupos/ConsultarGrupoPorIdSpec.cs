using Ardalis.Specification;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Specifications.Grupos
{
    public class ConsultarGrupoPorIdSpec : Specification<Grupo>, ISingleResultSpecification
    {
        public ConsultarGrupoPorIdSpec(int id)
        {
            Query
                .Where(grupo => grupo.Id == id)
                .AsNoTracking();
        }
    }
}