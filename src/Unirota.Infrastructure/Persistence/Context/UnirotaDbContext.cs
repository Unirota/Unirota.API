using Microsoft.EntityFrameworkCore;

namespace Unirota.Infrastructure.Persistence.Context;

public class UnirotaDbContext : DbContext
{
    public UnirotaDbContext(DbContextOptions options)
        : base(options)
    {
    }

    //TODO: Adicionar DbSets
    //ex: public DBSet<Entidade> Entidades => Set<Entidade>();
}
