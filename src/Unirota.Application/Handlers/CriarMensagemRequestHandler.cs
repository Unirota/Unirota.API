using MediatR;
using Unirota.Application.Services;
using Unirota.Application.Services.Mensagens;

namespace Unirota.Application.Commands.Mensagens
{
    public class CriarMensagemCommandHandler : IRequestHandler<CriarMensagemCommand, int>
    {
        private readonly IMensagemService _mensagemService;
        private readonly IServiceContext _serviceContext;

        public CriarMensagemCommandHandler(IMensagemService mensagemService, IServiceContext serviceContext)
        {
            _mensagemService = mensagemService;
            _serviceContext = serviceContext;
        }

        public async Task<int> Handle(CriarMensagemCommand request, CancellationToken cancellationToken)
        {
            var usuarioId = _serviceContext.UsuarioId; // Supondo que o ID do usuário esteja disponível no contexto de serviço
            return await _mensagemService.Criar(request, usuarioId);
        }
    }
}