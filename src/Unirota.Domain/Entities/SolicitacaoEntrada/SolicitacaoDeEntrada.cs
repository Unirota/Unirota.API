using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Domain.Entities.SolicitacoesDeEntrada;

public class SolicitacaoDeEntrada : BaseEntity, IAggregateRoot
{
    public int UsuarioId { get; set; }
    public int GrupoId { get; set; }
    
    public bool Aceito {get; set;}
    public Usuario Usuario { get; set; }
    public Grupo Grupo { get; set; }
}