﻿using Microsoft.EntityFrameworkCore;
using Unirota.Domain.Entities.Avaliacoes;
using Unirota.Domain.Entities.Corridas;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Enderecos;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Mensagens;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.UsuariosGrupos;
using Unirota.Domain.Entities.Veiculos;

namespace Unirota.Infrastructure.Persistence.Context;

public class UnirotaDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Corrida> Corridas => Set<Corrida>();
    public DbSet<Convite> Convites => Set<Convite>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<SolicitacaoDeEntrada> SolicitacaoDeEntrada => Set<SolicitacaoDeEntrada>();
    public DbSet<UsuariosGrupo> UsuariosGrupo => Set<UsuariosGrupo>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<Mensagem> Mensagens => Set<Mensagem>();
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
}
