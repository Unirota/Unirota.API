using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.Veiculos;

public interface IVeiculoService : IScopedService
{
    public Task<int> Criar(CriarVeiculosCommand request, CancellationToken cancellationToken);
}
