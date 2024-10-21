namespace Unirota.Application.ViewModels.Mensagens;

public class ListarMensagensViewModel
{
    public int Id { get; set; }
    public string Conteudo { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; }
    public DateTime CreatedAt { get; set; }
}