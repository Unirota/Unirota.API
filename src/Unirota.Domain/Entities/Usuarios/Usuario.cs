using Unirota.Domain.Common.Contracts;

namespace Unirota.Domain.Entities.Usuarios;

public class Usuario : BaseEntity, IAggregateRoot
{
    public string Nome { get; protected set; } 
}
