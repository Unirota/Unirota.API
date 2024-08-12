using Ardalis.Specification;
using Unirota.Domain.Common.Contracts;

namespace Unirota.Application.Persistence;

public interface IRepository<T> : IRepositoryBase<T>
where T : class, IAggregateRoot
{
}
