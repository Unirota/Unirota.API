namespace Unirota.Application.ViewModels.Grupos;

public class ListarGruposViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public string Motorista { get; set; }
    public string Destino { get; set; }
    public string UltimaMensagem { get; set; }
    public DateTime HoraInicio { get; set; }
    public double Nota { get; set; }
    public int Participantes { get; set; }
    public DateTime DataCriacao { get; set; }
}
