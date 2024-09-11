using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.Grupos;

public interface IGrupoService : IScopedService
{
    public Task<int> Criar(CriarGrupoCommand dto, int motoristaId);
}
