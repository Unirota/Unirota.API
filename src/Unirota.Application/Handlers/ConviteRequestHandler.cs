using MediatR;
using Unirota.Application.Commands.Convites;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Convites;
using Unirota.Application.Services;
using Unirota.Application.Services.Convites;
using Unirota.Application.Specifications.Convites;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;
public class ConviteRequestHandler : BaseRequestHandler,
                                     IRequestHandler<AceitarConviteCommand, bool>,
                                     IRequestHandler<CriarConviteCommand, int>,
                                     IRequestHandler<CancelarConvitePorIdCommand, bool>,
                                     IRequestHandler<RecusarConviteCommand, bool>,
                                     IRequestHandler<ObterConvitesUsuarioQuery, ICollection<ListarGruposViewModel>>
{
    private readonly IRepository<Convite> _repository;
    private readonly IReadRepository<Convite> _readRepository;
    private readonly IReadRepository<Usuario> _readUserRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IConviteService _service;


    public ConviteRequestHandler(IServiceContext serviceContext,
                                 IRepository<Convite> repository,
                                 IReadRepository<Convite> readRepository,
                                 IReadRepository<Usuario> readUserRepository,
                                 ICurrentUser currentUser,
                                 IConviteService conviteService) : base(serviceContext)
    {
        _repository = repository;
        _readRepository = readRepository;
        _service = conviteService;
        _currentUser = currentUser;
        _readUserRepository = readUserRepository;
    }
    public async Task<int> Handle(CriarConviteCommand request, CancellationToken cancellationToken)
    {
        var motorista = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(_currentUser.GetUserId()), cancellationToken);
        if (motorista is null)
        {
            ServiceContext.AddError("Motorista não encontrado");
            return default;
        }

        if (string.IsNullOrEmpty(motorista.Habilitacao))
        {
            ServiceContext.AddError("Motorista informado não possui habilitação cadastrada");
            return default;
        }

        var userConvidado = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorEmailSpec(request.Email), cancellationToken);
        if (userConvidado is null)
        {
            ServiceContext.AddError("Usuário não encontrado");
            return default;
        }

        var convite = await _service.Criar(userConvidado.Id, motorista.Id, request.GrupoId);
        return convite;
    }

    public async Task<bool> Handle(CancelarConvitePorIdCommand request, CancellationToken cancellationToken)
    {
        var convite = await _repository.FirstOrDefaultAsync(new ConsultarConvitePorIdSpec(request.Id), cancellationToken);
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


    public async Task<bool> Handle(AceitarConviteCommand request, CancellationToken cancellationToken)
    {
        return await _service.Aceitar(request);
    }

    public async Task<bool> Handle(RecusarConviteCommand request, CancellationToken cancellationToken)
    {
        int userId = _currentUser.GetUserId();
        Convite? convite = await _repository.FirstOrDefaultAsync(new ConsultarConvitePorIdSpec(request.Id), cancellationToken);
        if (convite is null)
        {
            ServiceContext.AddError("Convite não encontrado");
            return false;
        }
        if (convite.Aceito)
        {
            ServiceContext.AddError("Não é possível recusar um convite que já foi aceito");
            return false;
        }
        if (userId != convite.UsuarioId)
        {
            ServiceContext.AddError("Não é possível recusar um convite não direcionado a você");
            return false;
        }

        await _service.Cancelar(convite);
        return true;
    }

    public async Task<ICollection<ListarGruposViewModel>> Handle(ObterConvitesUsuarioQuery request, CancellationToken cancellationToken)
    {
        var convites = await _repository.ListAsync(new ObterConvitesPorUsuarioIdSpec(_currentUser.GetUserId()), cancellationToken);

        if (convites is null)
        {
            return [];
        }

        if (convites.Count == 0)
        {
            return [];
        }

        return convites.Select(x => new ListarGruposViewModel
        {
            Id = x.Id,
            Nome = x.Grupo.Nome,
            UltimaMensagem = x.Grupo.Mensagens.Count > 0 ? x.Grupo.Mensagens.OrderByDescending(y => y.CreatedAt).First().Conteudo : "",
            Descricao = x.Grupo.Descricao ?? "",
            Motorista = x.Motorista?.Nome ?? "",
            Destino = x.Grupo.Destino,
            HoraInicio = x.Grupo.HoraInicio,
            Participantes = x.Grupo.Passageiros.Count,
            DataCriacao = x.Grupo.CreatedAt,
            Nota = 5.0
        }).DistinctBy(x => x.Id).ToList();
    }
}
