using Ardalis.Specification;

namespace Unirota.Application.Specifications.Usuarios;

public class ConsultarUsuarioPorEmailSpec : Specification<Domain.Entities.Usuarios.Usuario>
{
    public ConsultarUsuarioPorEmailSpec(string email)
    {
        Query.Where(usuario => usuario.Email.ToLower() == email.ToLower());
    }
}
