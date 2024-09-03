namespace Unirota.Application.ViewModels.Usuarios;

public class UsuarioViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public bool Motorista { get; set; }
    
    public DateTime DataNascimento { get; set; }
}
