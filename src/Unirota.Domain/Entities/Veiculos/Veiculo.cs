using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.Veiculos;

public class Veiculo : BaseEntity, IAggregateRoot
{
   
        
    public string Placa { get; protected set; }
    public int MotoristaId { get; protected set; }
    public Usuario Motorista { get; set; }
    public string Cor { get; protected set; }
    public string Carroceria { get; protected set; }
    public string? Descricao { get; protected set; }

    public Veiculo (string placa, int motoristaId,string cor,string carroceria, string descricao)
    {
        Placa = placa;
        MotoristaId = motoristaId;
        Cor = cor;
        Carroceria = carroceria;
        Descricao = descricao;
    }
    
    public Veiculo()
    {

    }
}

