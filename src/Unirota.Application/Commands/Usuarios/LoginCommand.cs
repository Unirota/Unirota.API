using MediatR;
using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.Commands.Usuarios;

public class LoginCommand : IRequest<UsuarioViewModel>
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
