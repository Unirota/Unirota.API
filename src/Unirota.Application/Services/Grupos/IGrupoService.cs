using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services.Grupos;

public interface IGrupoService : IScopedService
{
    public Task<int> Criar(CriarGrupoCommand dto, int motoristaId);
    public Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId);
    public Task<bool> VerificarGrupoAtingiuLimiteUsuarios(int grupoId);
}
