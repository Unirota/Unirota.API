using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Domain.Entities.Corridas;

public class Corrida : BaseEntity, IAggregateRoot
{
    public int GrupoId { get; set; }
    public DateTime Comeco { get; set; }
    public DateTime Final { get; set; }

    public Corrida() { }

    public Corrida(int grupoId, DateTime comeco, DateTime final)
    {
        GrupoId = grupoId;
        Comeco = comeco;
        Final = final;
    }
}


