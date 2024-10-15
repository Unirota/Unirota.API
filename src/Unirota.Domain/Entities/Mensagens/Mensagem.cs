using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Domain.Entities.Mensagens;

public class Mensagem : BaseEntity, IAggregateRoot
{
    public string Conteudo { get; private set; }
    public int UsuarioId { get; private set; }
    public int GrupoId { get; private set; }
    
    public Grupo Grupo { get; private set; }

    
    public Mensagem(string conteudo, int usuarioId, int grupoId)
    {
        Conteudo = conteudo;
        UsuarioId = usuarioId;
        GrupoId = grupoId;
    }
    
}