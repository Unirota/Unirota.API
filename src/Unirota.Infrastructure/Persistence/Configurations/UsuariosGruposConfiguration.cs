using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class UsuariosGruposConfiguration : IEntityTypeConfiguration<UsuariosGrupo>
{
    public void Configure(EntityTypeBuilder<UsuariosGrupo> builder)
    {
        builder.ToTable("UsuariosGrupo");

        builder
            .HasKey(ug => new { ug.UsuarioId, ug.GrupoId });

        builder
            .HasOne(ug => ug.Usuario)
            .WithMany(u => u.UsuariosGrupos)
            .HasForeignKey(ug => ug.UsuarioId);

        builder
            .HasOne(ug => ug.Grupo)
            .WithMany(u => u.UsuariosGrupos)
            .HasForeignKey(ug => ug.GrupoId);
    }
}
