using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.UsuariosGrupos;

public class UsuariosGrupo : BaseEntity, IAggregateRoot
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int GrupoId { get; set; }
    public Grupo Grupo { get; set; }
}
