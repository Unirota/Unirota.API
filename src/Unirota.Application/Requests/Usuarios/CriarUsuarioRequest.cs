using MediatR;

namespace Unirota.Application.Requests.Usuarios;

public class CriarUsuarioRequest : IRequest<int>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Habilitacao { get; set; }
    public string CPF { get; set; }
}
