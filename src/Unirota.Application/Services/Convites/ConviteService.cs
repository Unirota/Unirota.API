using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Persistence;
using Unirota.Application.Specification.Convites;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Usuarios;

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

    public async Task<int> Criar(CriarConviteCommand dto)
    {
        Convite convite = new Convite(dto.UsuarioId, dto.MotoristaId, dto.GrupoId);
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

        convite.AceitarConvite();
        await _repository.SaveChangesAsync();
        
        return true;
    }

    public async Task Cancelar(Convite convite)
    {
        await _repository.DeleteAsync(convite);
    }

}

