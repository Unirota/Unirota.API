using MediatR;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Requests.Usuarios;
using Unirota.Application.Services;
using Unirota.Application.Specifications.Usuario;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;

public class UsuarioRequestHandler : BaseRequestHandler,
                                     IRequestHandler<CriarUsuarioCommand, int>
{
    private readonly IRepository<Usuario> _repository;

    public UsuarioRequestHandler(IServiceContext serviceContext, IRepository<Usuario> repository) : base(serviceContext)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var novoUsuario = new Usuario(request.Nome, request.Email, request.Habilitacao, request.Senha, request.CPF);

        await _repository.AddAsync(novoUsuario, cancellationToken);
        novoUsuario = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(novoUsuario.Id), cancellationToken);

        return novoUsuario.Id;
    }
}
