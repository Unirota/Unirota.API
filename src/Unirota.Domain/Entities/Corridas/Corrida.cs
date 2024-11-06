using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Domain.Entities.Corridas;

public class Corrida : BaseEntity, IAggregateRoot
{
    public int GrupoId { get; set; }
    public Grupo Grupo { get; set; }
    public DateTime Comeco { get; set; }
    public DateTime? Final { get; set; }

    public Corrida() { }

    public Corrida(int grupoId)
    {
        GrupoId = grupoId;
        Comeco = DateTime.UtcNow;
    }
}


