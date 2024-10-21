using MediatR;
using Unirota.Application.Commands.Corridas;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Services;
using Unirota.Application.Services.Corrida;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.ViewModels.Corrida;
using Unirota.Domain.Entities.Corridas;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Handlers;
public class CorridaRequestHandler : BaseRequestHandler,
                                    IRequestHandler<CriarCorridaCommand, int>
                                    IRequestHandler<CriarCorridaCommand, int>,
                                    IRequestHandler<ConsultarCorridaPorIdQuery, List<Corrida>>
{
    private readonly IReadRepository<Corrida> _readCorridaRepository;
    private readonly IReadRepository<Grupo> _readGrupoRepository;
    private readonly ICorridaService _service;
    private readonly ICurrentUser _currentUser;
    public CorridaRequestHandler(IServiceContext serviceContext,
                                ICurrentUser currentUser,
                                ICorridaService service,
                                IReadRepository<Corrida> readCorridaRepository,
                                IReadRepository<Grupo> readGrupoRepository) : base(serviceContext)
                               
    {
        _readCorridaRepository = readCorridaRepository;
        _readGrupoRepository = readGrupoRepository;
        _service = service;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CriarCorridaCommand request, CancellationToken cancellationToken)
    {
        var grupo = await _readGrupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(request.GrupoId), cancellationToken);

        if(grupo is null)
        {
            ServiceContext.AddError("Grupo não encontrado");
            return default;
        }
        if (grupo.MotoristaId != _currentUser.GetUserId()){
            ServiceContext.AddError("Você não está cadastrado como motorista");
            return default;
        }

        var corrida = await _service.Criar(request);
        return corrida;

    }

    public async Task<List<Corrida>> Handle(ConsultarCorridaPorIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.ObterPorIdDeGrupo(request, cancellationToken);

    }
}
