using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.Commands.Convites;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Commands.Corridas;
using Unirota.Application.Services;
using Unirota.Application.Persistence;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Corridas;
using Unirota.Application.Specification.Corridas;

namespace Unirota.Application.Handlers;
public class CorridaRequestHandler : BaseRequestHandler,
    IRequestHandler<CriarCorridaCommand, int>
{
    private readonly IReadRepository<Corrida> _readCorridaRepository;
    public CorridaRequestHandler(IServiceContext serviceContext,
                                IReadRepository<Corrida> readCorridaRepository)
                                : base(serviceContext)
    {
        _readCorridaRepository = readCorridaRepository;
    }

    public async Task<int> Handle(CriarCorridaCommand request, CancellationToken cancellationToken)
    {
        var corrida = await _readCorridaRepository.FirstOrDefaultAsync(new ConsultarCorridaPorIdSpec(request.GrupoId), cancellationToken);
    }
}
