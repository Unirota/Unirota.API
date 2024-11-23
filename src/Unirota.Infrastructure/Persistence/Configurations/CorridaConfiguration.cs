using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Corridas;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class CorridaConfiguration : IEntityTypeConfiguration<Corrida>
{
    public void Configure(EntityTypeBuilder<Corrida> builder)
    {
        builder.ToTable("Corridas");

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Avaliacoes)
               .WithOne()
               .HasForeignKey(x => x.CorridaId);
    }
}
