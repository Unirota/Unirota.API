using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.Covites;

public class Convite : BaseEntity, IAggregateRoot
{
    public int UsuarioId { get; set; }
    public int MotoristaId { get; set; }
    public int GrupoId { get; set; }
    public bool Aceito { get; set; }
    public Usuario Usuario { get; set; }
    public Usuario Motorista { get; set; }
    public Grupo Grupo { get; set; }

    public Convite() { }

    public Convite(int usuarioId, int motoristaId, int grupoId)
    {
        UsuarioId = usuarioId;
        MotoristaId = motoristaId;
        GrupoId = grupoId;
    }

    public Convite AceitarConvite(int usuarioId)
    {
        Aceito = true;
        Grupo.AdicionarPassageiro(usuarioId);
        return this;
    }
}

