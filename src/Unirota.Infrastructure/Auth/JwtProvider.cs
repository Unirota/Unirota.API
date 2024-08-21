using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.ViewModels.Auth;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Infrastructure.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public  JwtProvider(IOptions<JwtOptions> options)
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
