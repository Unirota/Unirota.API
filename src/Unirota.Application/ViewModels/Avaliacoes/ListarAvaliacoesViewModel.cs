namespace Unirota.Application.ViewModels.Avaliacoes;

public class ListarAvaliacoesViewModel
{
    public int Id { get; set; }
    public int Nota { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } 
    public DateTime DataAvaliacao { get; set; }
}