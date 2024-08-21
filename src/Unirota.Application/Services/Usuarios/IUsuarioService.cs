namespace Unirota.Application.Services.Usuarios;

public interface IUsuarioService
{
    string CriptografarSenha(string senha);

    bool ValidarSenha(string senha, string senhaCriptografada);
}
