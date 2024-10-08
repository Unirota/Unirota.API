using Unirota.Application.Commands.Convites;
using Unirota.Application.Common.Interfaces;
using Unirota.Domain.Entities.Covites;


namespace Unirota.Application.Services.Convites;

public interface IConviteService : IScopedService
{
    public Task<int> Criar(CriarConviteCommand dto);
    public Task<bool> Aceitar(AceitarConviteCommand dto);
    public Task Cancelar(Convite convite);

}

