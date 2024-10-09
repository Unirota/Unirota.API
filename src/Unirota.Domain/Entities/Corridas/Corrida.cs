using Unirota.Domain.Common.Contracts;

namespace Unirota.Domain.Entities.Corridas;

public class Corrida : BaseEntity, IAggregateRoot
{
    public int GrupoId { get; set; }
    public DateTime Comeco { get; set; }
    public DateTime Final { get; set; }

    public Corrida() { }

    public Corrida(int grupoId)
    {
        GrupoId = grupoId;
        Comeco = DateTime.UtcNow;
    }
}


