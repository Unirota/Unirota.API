using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Queries.Usuario;
using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.Services.Usuarios;

public interface IUsuarioService : IScopedService
{
    string CriptografarSenha(string senha);

    bool ValidarSenha(string senha, string senhaCriptografada);
    string ValidarCpf(string cpf);

    Task<UsuarioViewModel?> Editar(EditarUsuarioCommand usuario, CancellationToken cancellationToken);

    Task<UsuarioViewModel?> ConsultarPorId(int usuarioId, CancellationToken cancellationToken);
    
    Task<bool> VerificarUsuarioExiste(int usuarioId);

    Task<bool> CadastrarMotorista(string habilitacao);
}
