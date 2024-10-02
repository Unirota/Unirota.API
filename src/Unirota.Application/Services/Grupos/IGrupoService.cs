using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Services.Grupos;

public interface IGrupoService : IScopedService
{
    public Task<int> Criar(CriarGrupoCommand dto, int motoristaId);
    public Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId);
    public Task<bool> VerificarGrupoAtingiuLimiteUsuarios(int grupoId);
    public Task<bool> Deletar(DeletarGrupoCommand dto, Grupo grupo);
    public Task<ICollection<ListarGruposViewModel>> ObterPorUsuarioId(int usuarioId);
}
