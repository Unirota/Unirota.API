// using MediatR;
// using Unirota.Application.Common.Interfaces;
// using Unirota.Application.Handlers.Common;
// using Unirota.Application.Services;
// using Unirota.Application.Services.Avaliacoes;
//
// namespace Unirota.Application.Commands.Avaliacoes;
//
// public class AvaliacaoRequestHandler : BaseRequestHandler, IRequestHandler<CriarAvaliacaoCommand, int>
// {
//     private readonly IAvaliacaoService _avaliacaoService;
//     private readonly ICurrentUser _currentUser;
//
//     public AvaliacaoRequestHandler(IAvaliacaoService avaliacaoService, ICurrentUser currentUser, IServiceContext serviceContext) : base(serviceContext)
//     {
//         _avaliacaoService = avaliacaoService;
//         _currentUser = currentUser;
//     }
//
//     public async Task<int> Handle(CriarAvaliacaoCommand request, CancellationToken cancellationToken)
//     {
//         var usuarioId = _currentUser.GetUserId();
//         return await _avaliacaoService.Criar(request, usuarioId);
//     }
// }


using MediatR;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Services;
using Unirota.Application.Services.Avaliacoes;
using FluentValidation;

namespace Unirota.Application.Commands.Avaliacoes;

public class AvaliacaoRequestHandler : BaseRequestHandler, IRequestHandler<CriarAvaliacaoCommand, int>
{
    private readonly IAvaliacaoService _avaliacaoService;
    private readonly ICurrentUser _currentUser;
    private readonly IValidator<CriarAvaliacaoCommand> _validator; // Alterado para a interface

    public AvaliacaoRequestHandler(
        IAvaliacaoService avaliacaoService, 
        ICurrentUser currentUser, 
        IServiceContext serviceContext,
        IValidator<CriarAvaliacaoCommand> validator) : base(serviceContext)
    {
        _avaliacaoService = avaliacaoService;
        _currentUser = currentUser;
        _validator = validator; // Mantém como interface
    }

    public async Task<int> Handle(CriarAvaliacaoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var usuarioId = _currentUser.GetUserId();
        return await _avaliacaoService.Criar(request, usuarioId);
    }
}
