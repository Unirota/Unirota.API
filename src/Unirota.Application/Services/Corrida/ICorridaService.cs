using Unirota.Application.Commands.Corridas;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.Corrida;
public interface ICorridaService : IScopedService
{
    public Task<int> Criar(CriarCorridaCommand dto);
}

