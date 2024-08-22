using Microsoft.EntityFrameworkCore;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Infrastructure.Persistence.Context;

public class UnirotaDbContext : DbContext
{
    public UnirotaDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
}
