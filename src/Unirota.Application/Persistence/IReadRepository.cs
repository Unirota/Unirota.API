using Ardalis.Specification;
using Unirota.Domain.Common.Contracts;

namespace Unirota.Application.Persistence;

public interface IReadRepository<T> : IReadRepositoryBase<T> 
    where T : class, IAggregateRoot
{
}
