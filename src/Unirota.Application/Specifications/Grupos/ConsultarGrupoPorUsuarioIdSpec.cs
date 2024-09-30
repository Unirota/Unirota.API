using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unirota.Application.Specifications.Grupos
{
    public class ConsultarGrupoPorUsuarioIdSpec : Specification<Domain.Entities.Grupos.Grupo>
    {
        public ConsultarGrupoPorUsuarioIdSpec(int id)
        {
            Query
                .Where(grupo => grupo.Passageiros.Any(usuario => usuario.Id == id) || grupo.Motorista.Id.Equals(id))
                .AsNoTracking();
        }
    }
}
