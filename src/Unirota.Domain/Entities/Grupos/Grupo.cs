﻿using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Corridas;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Mensagens;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Domain.Entities.Grupos;

public class Grupo : BaseEntity, IAggregateRoot
{
    public string Nome { get; protected set; }
    public int MotoristaId { get; set; }
    public Usuario Motorista { get; set; }
    public int PassageiroLimite { get; protected set; }
    public string? ImagemUrl { get; protected set; }
    public DateTime HoraInicio { get; protected set; }
    public string? Descricao { get; protected set; }
    public string Destino { get; protected set; } 

    public ICollection<UsuariosGrupo> Passageiros { get; private set; } = new List<UsuariosGrupo>();

    public ICollection<SolicitacaoDeEntrada> SolicitacoesDeEntrada { get; private set; } = [];

    public ICollection<Mensagem> Mensagens { get; private set; } = [];
    public ICollection<Corrida> Corridas { get; set; } = [];
    public ICollection<Convite> Convites { get; set; } = [];

    public Grupo() { }

    public Grupo(string nome, int limite, DateTime inicio, int motoristaId, string destino)
    {
        Nome = nome;
        PassageiroLimite = limite;
        HoraInicio = inicio;
        MotoristaId = motoristaId;
        Destino = destino;
    }

    public Grupo AlterarDescricao(string descricao)
    {
        Descricao = descricao;
        return this;
    }

    public Grupo AlterarImagem(string url)
    {
        ImagemUrl = url;
        return this;
    }

    public Grupo AdicionarPassageiro(int usuarioId)
    {
        Passageiros.Add(new UsuariosGrupo
        {
            UsuarioId = usuarioId,
            GrupoId = Id
        });
        return this;
    }
}
