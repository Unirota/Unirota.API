using Unirota.Application.Commands.Corridas;
using Unirota.Application.Persistence;

namespace Unirota.Application.Services.Corrida;
public class CorridaService : ICorridaService
{
    private readonly IRepository<Domain.Entities.Corridas.Corrida> _repository;
    private readonly IServiceContext _serviceContext;

    public CorridaService(IRepository<Domain.Entities.Corridas.Corrida> repository, IServiceContext serviceContext)
    {
        _repository = repository;
        _serviceContext = serviceContext;
    }

    public async Task<int> Criar(CriarCorridaCommand dto)
    {
        var corrida = new Domain.Entities.Corridas.Corrida(dto.GrupoId);

        await _repository.AddAsync(corrida);
        return corrida.Id;
    }
}
