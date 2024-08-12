namespace Unirota.Domain.Common.Contracts;

public interface IEntity
{
    public int Id { get; }
    public DateTime CreatedAt { get; }
}
