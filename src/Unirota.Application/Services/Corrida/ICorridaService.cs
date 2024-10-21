using Unirota.Application.Commands.Corridas;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Queries.Corrida;


namespace Unirota.Application.Services.Corrida;
public interface ICorridaService : IScopedService
{
    public Task<int> Criar(CriarCorridaCommand dto);

    public Task<List<Domain.Entities.Corridas.Corrida?>> ObterPorIdDeGrupo(ConsultarCorridaPorIdQuery request, CancellationToken cancellationToken);
}

