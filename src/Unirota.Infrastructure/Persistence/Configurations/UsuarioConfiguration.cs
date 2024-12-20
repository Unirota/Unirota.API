﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(x => x.Id);

        builder.HasMany(u => u.GruposComoPassageiro)
            .WithOne(ug => ug.Usuario)
            .HasForeignKey(ug => ug.UsuarioId);

        builder.HasMany(u => u.GruposComoMotorista)
            .WithOne(g => g.Motorista)
            .HasForeignKey(g => g.MotoristaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SolicitacoesDeEntrada)
            .WithOne(g => g.Usuario)
            .HasForeignKey(h => h.UsuarioId);

    }
}
