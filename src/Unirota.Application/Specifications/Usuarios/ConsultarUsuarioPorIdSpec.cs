using Ardalis.Specification;

namespace Unirota.Application.Specifications.Usuarios;

public class ConsultarUsuarioPorIdSpec : Specification<Domain.Entities.Usuarios.Usuario>
{
    public ConsultarUsuarioPorIdSpec(int id)
    {
        Query.Where(usuario => usuario.Id.Equals(id));
    }
}
