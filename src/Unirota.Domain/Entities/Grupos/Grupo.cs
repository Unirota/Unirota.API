using Unirota.Domain.Common.Contracts;
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

    public ICollection<UsuariosGrupo> Passageiros { get; private set; } = new List<UsuariosGrupo>();

    public ICollection<SolicitacaoDeEntrada> SolicitacoesDeEntrada { get; private set; } = [];
    public Grupo() { }

    public Grupo(string nome, int limite, DateTime inicio, int motoristaId)
    {
        Nome = nome;
        PassageiroLimite = limite;
        HoraInicio = inicio;
        MotoristaId = motoristaId;
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

    public Grupo AdicionarPassageiro(Usuario usuario)
    {
        Passageiros.Add(new UsuariosGrupo
        {
            UsuarioId = usuario.Id,
            GrupoId = Id
        });
        return this;
    }
}
