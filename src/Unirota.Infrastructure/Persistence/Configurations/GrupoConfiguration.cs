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

        builder.HasOne(g => g.Motorista)
            .WithMany(u => u.GruposComoMotorista)
            .HasForeignKey(g => g.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Passageiros)
            .WithOne(ug => ug.Grupo)
            .HasForeignKey(ug => ug.GrupoId);

        builder.HasMany(u => u.SolicitacoesDeEntrada)
            .WithOne(h => h.Grupo)
            .HasForeignKey(j => j.GrupoId);

        builder.HasMany(a => a.Mensagens)
            .WithOne(b => b.Grupo)
            .HasForeignKey(c => c.GrupoId);

        builder.HasMany(a => a.Corridas)
            .WithOne(b => b.Grupo)
            .HasForeignKey(x => x.GrupoId);
    }
}
