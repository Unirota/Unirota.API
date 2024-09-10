using MediatR;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Convite;

using Unirota.Application.Services;
using Unirota.Application.Services.Convites;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;
public class ConviteRequestHandler : BaseRequestHandler,
    IRequestHandler<CriarConviteCommand, int>,
    IRequestHandler<ConsultarConvitePorIdQuery, bool>
{
    private readonly IRepository<Convite> _repository;
    private readonly IReadRepository<Convite> _readRepository;
    private readonly IReadRepository<Usuario> _readUserRepository;
    private readonly IConviteService _service;


    public ConviteRequestHandler(IServiceContext serviceContext,
                                 IRepository<Convite> repository,
                                 IReadRepository<Convite> readRepository,
                                 IReadRepository<Usuario> readUserRepository,
                                 IConviteService conviteService) : base(serviceContext)
    {
        _repository = repository;
        _readRepository = readRepository;
        _service = conviteService;
        _readUserRepository = readUserRepository;
    }
    public async Task<int> Handle(CriarConviteCommand request, CancellationToken cancellationToken)
    {
        var motorista = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(request.MotoristaId), cancellationToken);
        if (motorista is null)
        {
            ServiceContext.AddError("Motorista não encontrado");
            return default;
        }

        if (motorista.Habilitacao is null)
        {
            ServiceContext.AddError("Motorista informado não possui habilitação cadastrada");
            return default;
        }

        var userConvidado = await _readUserRepository.AnyAsync(new ConsultarUsuarioPorIdSpec(request.UsuarioId), cancellationToken);
        if (!userConvidado)
        {
            ServiceContext.AddError("Usuário não encontrado");
            return default;
        }

        var convite = await _service.Criar(request);
        return convite;
    }

    public async Task<bool> Handle(ConsultarConvitePorIdQuery request, CancellationToken cancellation)
    {
        var convite = await _repository.GetByIdAsync(request.Id);
        if (convite is null)
        {
            ServiceContext.AddError("Convite não encontrado");
            return false;
        }
        if (convite.Aceito)
        {
            ServiceContext.AddError("Não é possível cancelar um convite que já foi aceito");
            return false;
        }

        await _service.Cancelar(convite);
        return true;
    }

}

