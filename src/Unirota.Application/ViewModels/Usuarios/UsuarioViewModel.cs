namespace Unirota.Application.ViewModels.Usuarios;

public class UsuarioViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Habilitacao { get; set; }
    public string? CPF { get; set; }
}
