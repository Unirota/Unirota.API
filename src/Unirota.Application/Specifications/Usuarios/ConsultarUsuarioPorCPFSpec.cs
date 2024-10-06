using Ardalis.Specification;

public class ConsultarUsuarioPorCPFSpec : Specification<Unirota.Domain.Entities.Usuarios.Usuario>
{ 
    public ConsultarUsuarioPorCPFSpec(string cpf)
    {
        Query.Where(usuario => usuario.CPF == cpf);
    }
}
