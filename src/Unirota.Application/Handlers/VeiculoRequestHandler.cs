using MediatR;
using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Services;
using Unirota.Application.Services.Veiculos;

namespace Unirota.Application.Handlers;

public class VeiculoRequestHandler : BaseRequestHandler,
                                     IRequestHandler<CriarVeiculosCommand, int>
{
    private readonly IVeiculoService _veiculoService;

    public VeiculoRequestHandler(IServiceContext serviceContext, IVeiculoService veiculoService) : base(serviceContext)
    {
        _veiculoService = veiculoService;
    }

    public async Task<int> Handle(CriarVeiculosCommand request, CancellationToken cancellationToken)
    {
        return await _veiculoService.Criar(request, cancellationToken);
    }
}
