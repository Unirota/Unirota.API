using Ardalis.Specification;
using Unirota.Domain.Entities.Veiculos;

namespace Unirota.Application.Specifications.Veiculos;

public class ConsultarVeiculoPorPlacaSpec : Specification <Veiculo>
{
    public ConsultarVeiculoPorPlacaSpec(string placa)
    {
        Query.Where(veiculo => veiculo.Placa.ToLower() == placa.ToLower());
    }
}
