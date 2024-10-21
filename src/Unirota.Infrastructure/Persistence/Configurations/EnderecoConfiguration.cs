using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Enderecos;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{

    public void Configure(EntityTypeBuilder<Endereco> modelBuilder)
    {
        modelBuilder.ToTable("Enderecos");
        
        modelBuilder.HasKey(x => x.Id);
    }
}
