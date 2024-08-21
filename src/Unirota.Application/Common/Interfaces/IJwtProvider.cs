using Unirota.Application.ViewModels.Auth;
using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.Common.Interfaces;

public interface IJwtProvider
{
    public TokenViewModel GerarToken(UsuarioViewModel usuario);
}
