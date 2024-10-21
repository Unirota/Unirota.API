namespace Unirota.Application.ViewModels;

public class ResultadoPaginadoViewModel<T>
{
    public ResultadoPaginadoViewModel(int quantidadeRegistros, ICollection<T> itens)
    {
        QuantidadeRegistros = quantidadeRegistros;
        Itens = itens;
    }
    
    public int QuantidadeRegistros { get; private set; }

    public ICollection<T> Itens { get; private set; }
}
