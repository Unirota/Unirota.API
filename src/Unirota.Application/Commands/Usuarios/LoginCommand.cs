using MediatR;
using Unirota.Application.ViewModels.Auth;

namespace Unirota.Application.Commands.Usuarios;

public class LoginCommand : IRequest<TokenViewModel>
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
