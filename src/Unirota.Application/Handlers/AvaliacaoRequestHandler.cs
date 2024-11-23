using FluentValidation;
using MediatR;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Services;
using Unirota.Application.Services.Avaliacoes;

namespace Unirota.Application.Handlers;

public class AvaliacaoRequestHandler(IAvaliacaoService avaliacaoService,
                                     ICurrentUser currentUser,
                                     IServiceContext serviceContext,
                                     IValidator<CriarAvaliacaoCommand> validator)
                                     : BaseRequestHandler(serviceContext), IRequestHandler<CriarAvaliacaoCommand, int>
{
    public async Task<int> Handle(CriarAvaliacaoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var usuarioId = currentUser.GetUserId();
        return await avaliacaoService.Criar(request, usuarioId);
    }
}
