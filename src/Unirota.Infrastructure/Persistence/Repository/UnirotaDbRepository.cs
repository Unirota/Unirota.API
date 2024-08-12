using Ardalis.Specification.EntityFrameworkCore;
using Unirota.Application.Persistence;
using Unirota.Domain.Common.Contracts;
using Unirota.Infrastructure.Persistence.Context;

namespace Unirota.Infrastructure.Persistence.Repository;

public class UnirotaDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public UnirotaDbRepository(UnirotaDbContext context)
        : base(context)
    {

    }
}
