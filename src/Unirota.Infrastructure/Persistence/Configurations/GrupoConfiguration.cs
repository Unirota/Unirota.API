using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
{
    public void Configure(EntityTypeBuilder<Grupo> builder)
    {
        builder.ToTable("Grupos");

        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Motorista)
            .WithMany()
            .HasForeignKey(x => x.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
