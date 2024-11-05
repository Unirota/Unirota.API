using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Corridas;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.Avaliacoes;

public class Avaliacao : BaseEntity, IAggregateRoot
{
    public int UsuarioId { get; set; }
    
    public int CorridaId { get; set; }
    public int Nota { get; set; }
    
    public Usuario Usuario { get; set; }
    public Corrida Corrida { get; set; }

    public Avaliacao(int nota, int usuarioId, int corridaId)
    {
        Nota = nota;
        UsuarioId = usuarioId;
        CorridaId = corridaId;
    }

    public Avaliacao() { }
}