using Mapster;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Usuario;
using Unirota.Application.Specification.Usuarios;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Shared;

namespace Unirota.Application.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly IRepository<Usuario> _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IServiceContext _serviceContext;

    public UsuarioService(IRepository<Usuario> repository,
                          ICurrentUser currentUser,
                          IServiceContext serviceContext)
    {
        _repository = repository;
        _currentUser = currentUser;
        _serviceContext = serviceContext;
    }

    public string CriptografarSenha(string senha)
    {
        return HashHelper.Hash(senha);
    }

    public bool ValidarSenha(string senha, string senhaCriptografada)
    {
        return HashHelper.ValidarSenha(senha, senhaCriptografada);
    }

    public async Task<UsuarioViewModel?> Editar(EditarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var current = _currentUser.GetUserId();

        if (current != request.Id)
        {
            _serviceContext.AddError("Usuário não pode editar outro usuário.");
            return null;
        }

        var usuarioDb = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(request.Id), cancellationToken);

        if(usuarioDb == null)
        {
            _serviceContext.AddError("Usuário não cadastrado.");
            return null;
        }

        if(!string.IsNullOrEmpty(request.Nome))
        {
            usuarioDb
                .AlterarNome(request.Nome);
        }

        if (request.DataNascimento.HasValue)
        {
            usuarioDb
                .AlterarDataNascimento(request.DataNascimento.Value);
        }

        if (!string.IsNullOrEmpty(request.Senha))
        {
            var senhaCriptografada = CriptografarSenha(request.Senha);

            usuarioDb
                .AlterarSenha(senhaCriptografada);
        }

        if(!string.IsNullOrEmpty(request.ImagemUrl))
        {
            usuarioDb
                .AlterarImagem(request.ImagemUrl);
        }

        await _repository.SaveChangesAsync(cancellationToken);

        return usuarioDb.Adapt<UsuarioViewModel>();
    }

    public async Task<UsuarioViewModel?> ConsultarPorId(int usuarioId, CancellationToken cancellationToken)
    {
        var usuario = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(usuarioId), cancellationToken);
        
        if (usuario is null)
        {
            _serviceContext.AddError("Usuário não existente");
            return null;
        }

        return usuario.Adapt<UsuarioViewModel>();
    }
}
