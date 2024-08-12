namespace Unirota.Domain.Common.Contracts;

public abstract class BaseEntity : IEntity
{
    protected BaseEntity()
    {
        CreatedAt = DateTime.Now;
    }

    protected BaseEntity(int id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
    }

    public int Id { get; }
    public DateTime CreatedAt { get; private set; }
}
