using Microsoft.EntityFrameworkCore;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Infrastructure.Persistence.Context;

public class UnirotaDbContext : DbContext
{
    public UnirotaDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Convite> Convites => Set<Convite>();

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Usuario>().ToTable("usuarios");
    //    base.OnModelCreating(modelBuilder);
    //}
}
