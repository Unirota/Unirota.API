using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.ViewModels.Auth;

public class TokenViewModel
{
    public string AccessToken { get; set; }
    public string TokenType { get; private set; } = "Bearer";
    public UsuarioViewModel Usuario { get; set; }
}
