using Ardalis.Specification;

namespace Unirota.Application.Specifications.Grupos;

public class ConsultarGrupoComoPassageiroSpec : Specification<Domain.Entities.Grupos.Grupo>
{
    public ConsultarGrupoComoPassageiroSpec(int usuarioId)
    {
        Query
            .Where(grupo => grupo.Passageiros.Any(passageiro => passageiro.UsuarioId == usuarioId))
            .Include(x => x.Passageiros)
            .Include(x => x.Corridas)
            .Include(x => x.Motorista);
    }
}
