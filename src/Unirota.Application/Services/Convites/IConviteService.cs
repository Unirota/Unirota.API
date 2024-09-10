using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.Convites;

public interface IConviteService : IScopedService
{
    public Task<int> Criar(CriarConviteCommand dto);
}

