using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.SolicitacoesDeEntrada;

public class SolicitacaoDeEntrada : BaseEntity, IAggregateRoot
{
    public int UsuarioId { get; private set; }
    public int GrupoId { get; private set; }
    
    public bool Aceito {get; private set;}
    public Usuario Usuario { get; set; }
    public Grupo Grupo { get; set; }

    public SolicitacaoDeEntrada(int usuarioId, int grupoId)
    {
        UsuarioId = usuarioId;
        GrupoId = grupoId;
        Aceito = false;
    }

    public void Aceitar()
    {
        Aceito = true;
    }
}