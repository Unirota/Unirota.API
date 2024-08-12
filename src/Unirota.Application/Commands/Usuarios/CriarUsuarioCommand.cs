using MediatR;

namespace Unirota.Application.Requests.Usuarios;

public class CriarUsuarioCommand : IRequest<int>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Habilitacao { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
}
