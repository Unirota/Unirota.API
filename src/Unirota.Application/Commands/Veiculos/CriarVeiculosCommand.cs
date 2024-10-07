using MediatR;

namespace Unirota.Application.Commands.Veiculos;

public class CriarVeiculosCommand : IRequest <int>
{
    public string Placa { get; set; }
    public string Descricao { get; set; }
    public string Carroceria { get; set; }
    public string Cor {  get; set; }

}
