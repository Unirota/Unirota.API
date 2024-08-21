using Unirota.Shared;

namespace Unirota.Application.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
    public string CriptografarSenha(string senha)
    {
        return HashHelper.Hash(senha);
    }

    public bool ValidarSenha(string senha, string senhaCriptografada)
    {
        return HashHelper.ValidarSenha(senha, senhaCriptografada);
    }
}
