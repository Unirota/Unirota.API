using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Persistence;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Services.Convites;

public class ConviteService :IConviteService
{
    private readonly IRepository<Convite> _repository;

    public ConviteService(IRepository<Convite> repository)
    {
        _repository = repository;
    }

    public async Task<int> Criar(CriarConviteCommand dto)
    {
        Convite convite = new Convite(dto.UsuarioId, dto.MotoristaId, dto.GrupoId);
        await _repository.AddAsync(convite);
        return convite.Id;
    }

    public async Task Cancelar(Convite convite)
    {
        await _repository.DeleteAsync(convite);
    }

}

