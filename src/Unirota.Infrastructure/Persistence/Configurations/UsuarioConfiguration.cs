using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Grupos");

        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Grupos)
            .WithMany(x => x.Passageiros)
            .UsingEntity(x => x.ToTable("UsuariosGrupo"));
    }
}
