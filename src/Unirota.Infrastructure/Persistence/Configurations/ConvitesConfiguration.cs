using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Covites;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class ConvitesConfiguration : IEntityTypeConfiguration<Convite>
{
    public void Configure(EntityTypeBuilder<Convite> builder)
    {
        builder.ToTable("Convites");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId);

        builder.HasOne(x => x.Motorista)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId);

        builder.HasOne(x => x.Grupo)
            .WithMany()
            .HasForeignKey(x => x.GrupoId);
    }
}
