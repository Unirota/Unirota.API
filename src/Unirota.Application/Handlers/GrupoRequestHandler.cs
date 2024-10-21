using MediatR;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;
namespace Unirota.Application.Handlers;

public class GrupoRequestHandler : BaseRequestHandler,
                                   IRequestHandler<CriarGrupoCommand, int>,
                                   IRequestHandler<DeletarGrupoCommand, bool>,
                                   IRequestHandler<ObterGrupoUsuarioCommand, ICollection<ListarGruposViewModel>>,
                                   IRequestHandler<ConsultarGrupoPorIdQuery, Grupo>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Usuario> _readUserRepository;
    private readonly IReadRepository<Grupo> _readGrupoRepository;
    private readonly IGrupoService _service;

    public GrupoRequestHandler(IServiceContext serviceContext,
                               ICurrentUser currentUser,
                               IGrupoService service,
                               IReadRepository<Usuario> readUserRepository,
                               IReadRepository<Grupo> readGrupoRepository) : base(serviceContext)

    {
        _currentUser = currentUser;
        _readUserRepository = readUserRepository;
        _readGrupoRepository = readGrupoRepository;
        _service = service;

    }

    public async Task<int> Handle(CriarGrupoCommand request, CancellationToken cancellationToken)
    {
        var motorista = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(_currentUser.GetUserId()), cancellationToken);
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

        var grupo = await _service.Criar(request, motorista.Id);
        return grupo;


    }
    public async Task<int> Handle(EditarGrupoCommand request, CancellationToken cancellationToken)
    {
        return await _service.Editar(request, cancellationToken);
    }

    public async Task<Grupo> Handle(ConsultarGrupoPorIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.ObterPorId(request, cancellationToken);
    }

    public async Task<bool> Handle(DeletarGrupoCommand request, CancellationToken cancellationToken)
    {
        var motorista = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(_currentUser.GetUserId()), cancellationToken);

        if (motorista is null)
        {
            ServiceContext.AddError("Motorista não encontrado");
            return false;
        }

        var grupo = await _readGrupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(request.Id), cancellationToken);

        if (grupo is null)
        {
            ServiceContext.AddError("Grupo não encontrado");
            return false;
        }

        if (!motorista.Id.Equals(grupo.MotoristaId))
        {
            ServiceContext.AddError("O usuário está tentando apagar um grupo que não é motorista");
            return false;
        }

        await _service.Deletar(request, grupo);

        return true;
    }

    public async Task<ICollection<ListarGruposViewModel>> Handle(ObterGrupoUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(_currentUser.GetUserId()), cancellationToken);

        if (usuario is null)
        {
            ServiceContext.AddError("Usuário não encontrado");
            return [];
        }

        if(usuario.GruposComoMotorista is null)
        {
            ServiceContext.AddError("Este usuário não tem grupos");
            return [];
        }

        if(_currentUser.GetUserId() != request.Id)
        {
            ServiceContext.AddError("Usuário não pode consultar grupos de outro usuário");
            return [];
        }

        
        return await _service.ObterPorUsuarioId(request.Id);;
    }
}
