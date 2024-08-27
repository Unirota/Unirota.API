using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.Services.Usuarios;

public interface IUsuarioService : IScopedService
{
    string CriptografarSenha(string senha);

    bool ValidarSenha(string senha, string senhaCriptografada);

    Task<UsuarioViewModel?> Editar(EditarUsuarioCommand usuario, CancellationToken cancellationToken);
}
