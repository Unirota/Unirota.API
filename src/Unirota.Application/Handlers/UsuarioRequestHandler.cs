using Mapster;
using MediatR;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Requests.Usuarios;
using Unirota.Application.Services;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Auth;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;

public class UsuarioRequestHandler : BaseRequestHandler,
                                     IRequestHandler<CriarUsuarioCommand, int>,
                                     IRequestHandler<LoginCommand, TokenViewModel>
{
    private readonly IRepository<Usuario> _repository;
    private readonly IReadRepository<Usuario> _readRepository;
    private readonly IUsuarioService _service;
    private readonly IJwtProvider _jwtProvider;

    public UsuarioRequestHandler(IServiceContext serviceContext,
                                 IRepository<Usuario> repository,
                                 IReadRepository<Usuario> readRepository,
                                 IUsuarioService usuarioService,
                                 IJwtProvider jwtProvider) : base(serviceContext)
    {
        _repository = repository;
        _service = usuarioService;
        _readRepository = readRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<int> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var senhaCriptografada = _service.CriptografarSenha(request.Senha);
        var novoUsuario = new Usuario(request.Nome, request.Email, request.Habilitacao, senhaCriptografada, request.CPF);

        await _repository.AddAsync(novoUsuario, cancellationToken);
        novoUsuario = await _readRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(novoUsuario.Id), cancellationToken);
        
        return novoUsuario.Id;
    }

    public async Task<TokenViewModel?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _readRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorEmailSpec(request.Email), cancellationToken);

        if(usuario == null)
        {
            ServiceContext.AddError("Usuário não encontrado");
            return default;
        }

        if(!_service.ValidarSenha(request.Senha, usuario.Senha))
        {
            ServiceContext.AddError("Senha inválida");
            return default;
        }

        return _jwtProvider.GerarToken(usuario.Adapt<UsuarioViewModel>());
    }
}
