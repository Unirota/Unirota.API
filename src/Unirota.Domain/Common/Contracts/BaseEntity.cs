namespace Unirota.Domain.Common.Contracts;

public abstract class BaseEntity : IEntity
{
    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected BaseEntity(int id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; protected set; }
    public DateTime CreatedAt { get; private set; }
}
