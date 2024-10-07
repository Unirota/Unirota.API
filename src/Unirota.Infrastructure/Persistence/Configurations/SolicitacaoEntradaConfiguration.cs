using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class SolicitacaoEntradaConfiguration
{
    public void Configure(EntityTypeBuilder<SolicitacaoDeEntrada> builder)
    {
        builder.ToTable("SolicitacaoDeEntrada");

        builder.HasKey(x => x.Id);

        builder.HasOne(e => e.Usuario)
            .WithMany(w => w.SolicitacoesDeEntrada)
            .HasForeignKey(f => f.UsuarioId);

        builder.HasOne(e => e.Grupo)
            .WithMany(w => w.SolicitacoesDeEntrada)
            .HasForeignKey(z => z.GrupoId);
    }
}