using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Grupos)
            .WithMany(x => x.Passageiros)
            .UsingEntity<UsuariosGrupo>(x => x.ToTable("UsuariosGrupo"));
    }
}
