using Unirota.Application.Commands.Grupos;
using Unirota.Application.Commands.Grupos.ObterGrupoUsuario;
using Unirota.Application.Common.Interfaces;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Services.Grupos;

public interface IGrupoService : IScopedService
{
    public Task<int> Criar(CriarGrupoCommand dto, int motoristaId);
    public Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId);
    public Task<bool> VerificarGrupoAtingiuLimiteUsuarios(int grupoId);
    public Task<bool> Deletar(DeletarGrupoCommand dto, Grupo grupo);
    public Task<ICollection<Grupo>> ObterPorUsuarioId(int usuarioId);
}
