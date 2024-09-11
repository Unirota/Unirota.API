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
            .HasMany(x => x.Passageiros)
            .WithMany(x => x.Grupos)
            .UsingEntity(x => x.ToTable("UsuariosGrupo"));
    }
}
