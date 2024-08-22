using MediatR;

namespace Unirota.Application.Commands.Usuarios;

public class CriarUsuarioCommand : IRequest<int>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
}
