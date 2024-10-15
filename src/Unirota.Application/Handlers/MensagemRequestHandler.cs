using MediatR;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Services;
using Unirota.Application.Services.Mensagens;

namespace Unirota.Application.Commands.Mensagens;

public class MensagemRequestHandler : BaseRequestHandler, IRequestHandler<CriarMensagemCommand, int>
{
    private readonly IMensagemService _mensagemService;
    private readonly ICurrentUser _currentUser;

    public MensagemRequestHandler(IMensagemService mensagemService, ICurrentUser currentUser, IServiceContext serviceContext) : base(serviceContext)
    {
        _mensagemService = mensagemService;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CriarMensagemCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.GetUserId(); 
        return await _mensagemService.Criar(request, usuarioId);
    }
}
