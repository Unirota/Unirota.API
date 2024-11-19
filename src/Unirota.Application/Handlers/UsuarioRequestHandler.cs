using Mapster;
using MediatR;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Usuario;
using Unirota.Application.Services;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Auth;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Domain.Entities.Enderecos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;

public class UsuarioRequestHandler : BaseRequestHandler,
                                     IRequestHandler<CriarUsuarioCommand, int>,
                                     IRequestHandler<LoginCommand, TokenViewModel>,
                                     IRequestHandler<EditarUsuarioCommand, UsuarioViewModel>,
                                     IRequestHandler<ConsultarUsuarioPorIdQuery, UsuarioViewModel>,
                                     IRequestHandler<CadastrarMotoristaCommand, bool>
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
        string cpfValidationResult = _service.ValidarCpf(request.CPF);
        if (cpfValidationResult != "Ok")
        {
            ServiceContext.AddError(cpfValidationResult);
            return default;
        }
        
        var cpfDuplicado = await _readRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorCPFSpec(request.CPF), cancellationToken);
        if (cpfDuplicado != null)
        {
            ServiceContext.AddError("CPF duplicado!");
            return default;
        }

        var senhaCriptografada = _service.CriptografarSenha(request.Senha);

        var novoUsuario = new Usuario(request.Nome, request.Email, senhaCriptografada, request.CPF, request.DataNascimento);
        var enderecoUsuario = new Endereco(request.Endereco.CEP,
                                           request.Endereco.Logradouro,
                                           request.Endereco.Numero,
                                           request.Endereco.Cidade,
                                           request.Endereco.Bairro,
                                           request.Endereco.UF,
                                           novoUsuario.Id);
        enderecoUsuario.AlterarComplemento(request.Endereco.Complemento);
        
        novoUsuario.AlterarEndereco(enderecoUsuario);

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

    public async Task<UsuarioViewModel> Handle(EditarUsuarioCommand request, CancellationToken cancellationToken)
    {
        return await _service.Editar(request, cancellationToken);
    }

    public async Task<UsuarioViewModel> Handle(ConsultarUsuarioPorIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.ConsultarPorId(request.Id, cancellationToken);
    }

    public async Task<bool> Handle(CadastrarMotoristaCommand request, CancellationToken cancellationToken)
    {
        return await _service.CadastrarMotorista(request.Habilitacao);
    }
}
