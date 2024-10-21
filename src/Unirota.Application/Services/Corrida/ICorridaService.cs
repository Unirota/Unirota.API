using Unirota.Application.Commands.Corridas;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Specification.Corridas;
using Unirota.Domain.Entities.Corridas;

namespace Unirota.Application.Services.Corrida;
public interface ICorridaService : IScopedService
{
    public Task<int> Criar(CriarCorridaCommand dto);

    public Task<List<Domain.Entities.Corridas.Corrida?>> ObterPorIdDeGrupo(ConsultarCorridaPorIdQuery request, CancellationToken cancellationToken);
}

