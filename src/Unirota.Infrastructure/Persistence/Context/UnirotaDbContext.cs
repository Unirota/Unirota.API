using Microsoft.EntityFrameworkCore;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Infrastructure.Persistence.Context;

public class UnirotaDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Convite> Convites => Set<Convite>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
}
