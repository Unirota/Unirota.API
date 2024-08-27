using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Auth;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Infrastructure.Auth;

namespace Unirota.Application.Common;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public TokenViewModel GerarToken(UsuarioViewModel usuario)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, null, DateTime.UtcNow.AddHours(24), signingCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenViewModel { AccessToken = tokenString, Usuario = usuario };
    }
}
