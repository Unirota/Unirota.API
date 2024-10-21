using Unirota.Application.Commands.Corridas;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Specification.Corridas;

namespace Unirota.Application.Services.Corrida;
public class CorridaService : ICorridaService
{
    private readonly IRepository<Domain.Entities.Corridas.Corrida> _repository;
    private readonly IReadRepository<Domain.Entities.Corridas.Corrida> _readrepository;
    private readonly IServiceContext _serviceContext;

    public CorridaService(IRepository<Domain.Entities.Corridas.Corrida> repository, IServiceContext serviceContext, IReadRepository<Domain.Entities.Corridas.Corrida> readRepository)
    {
        _repository = repository;
        _serviceContext = serviceContext;
        _readrepository = readRepository;
    }

    public async Task<int> Criar(CriarCorridaCommand dto)
    {
        var corrida = new Domain.Entities.Corridas.Corrida(dto.GrupoId);

        await _repository.AddAsync(corrida);
        return corrida.Id;
    }

    public async Task<List<Domain.Entities.Corridas.Corrida?>> ObterPorIdDeGrupo(ConsultarCorridaPorIdQuery request, CancellationToken cancellationToken)
    {
        var corridas = await _readrepository.ListAsync(new ConsultarCorridaPorIdSpec(request.Id), cancellationToken);
        
        if(corridas == null)
            _serviceContext.AddError("Não existe corridas para este grupo");
        return corridas;

    }
}
