using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Application.Services.Convites;

public interface IConviteService : IScopedService
{
    public Task<int> Criar(CriarConviteCommand dto);
    public Task Cancelar(Convite convite);
}

