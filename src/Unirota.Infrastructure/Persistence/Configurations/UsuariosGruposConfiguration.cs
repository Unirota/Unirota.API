using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class UsuariosGruposConfiguration : IEntityTypeConfiguration<UsuariosGrupo>
{
    public void Configure(EntityTypeBuilder<UsuariosGrupo> builder)
    {
        builder.ToTable("UsuariosGrupo");

        builder.HasKey(x => x.Id);

        builder
           .HasOne(ug => ug.Usuario)
           .WithMany(u => u.GruposComoPassageiro)
           .HasForeignKey(ug => ug.UsuarioId);

        builder
            .HasOne(ug => ug.Grupo)
            .WithMany(g => g.Passageiros)
            .HasForeignKey(ug => ug.GrupoId);

    }
}
