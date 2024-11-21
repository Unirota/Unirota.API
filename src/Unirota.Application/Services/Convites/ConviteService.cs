using Unirota.Application.Commands.Convites;
using Unirota.Application.Persistence;
using Unirota.Application.Specifications.Convites;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Services.Convites;

public class ConviteService : IConviteService
{
    private readonly IRepository<Convite> _repository;
    private readonly IServiceContext _serviceContext;

    public ConviteService(IRepository<Convite> repository, IServiceContext serviceContext)
    {
        _repository = repository;
        _serviceContext = serviceContext;
    }

    public async Task<int> Criar(int usuarioId, int motoristaId, int grupoId)
    {
        var conviteExistente = await _repository.FirstOrDefaultAsync(
            new ConsultarConvitePorIdSpec(usuarioId, motoristaId, grupoId));

        if (conviteExistente != null)
        {
            _serviceContext.AddError("Já existe um convite pendente para este usuário e motorista.");
            return 0;
        }
        
        Convite convite = new Convite(usuarioId, motoristaId, grupoId);
        await _repository.AddAsync(convite);
        return convite.Id;
    }

    public async Task<bool> Aceitar(AceitarConviteCommand dto)
    {
        var convite = await _repository.FirstOrDefaultAsync(new ConsultarConvitePorIdSpec(dto.Id));

        if (convite is null)
        {
            _serviceContext.AddError("Convite não encontrado");
            return false;
        }

        convite.AceitarConvite(convite.UsuarioId);
        await _repository.SaveChangesAsync();

        await _repository.DeleteAsync(convite);
        return true;
    }

    public async Task Cancelar(Convite convite)
    {
        await _repository.DeleteAsync(convite);
    }
}

