using Unirota.Application.ViewModels.Enderecos;

namespace Unirota.Application.ViewModels.Usuarios;

public class UsuarioViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public bool Motorista { get; set; }
    public EnderecoViewModel Endereco { get; set; }
    public int Corridas { get; set; }
    public DateTime DataNascimento { get; set; }
    public DateTime CreatedAt { get; set; }
}
