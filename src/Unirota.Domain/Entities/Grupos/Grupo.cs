using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.Grupos;

public class Grupo : BaseEntity, IAggregateRoot
{
    public string Nome { get; protected set; }
    public int MotoristaId { get; protected set; }
    public Usuario Motorista { get; protected set; }
    public int PassageiroLimite { get; protected set; }
    public string? ImagemUrl { get; protected set; }
    public DateTime HoraInicio { get; protected set; }
    public string? Descricao { get; protected set; }

    private readonly List<Usuario> _passageiros = [];
    public IReadOnlyList<Usuario> Passageiros => _passageiros.AsReadOnly();

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
        _passageiros.Add(usuario);
        usuario.AdicionarGrupo(this);
        return this;
    }

    public Grupo RemoverPassageiro(Usuario usuario)
    {
        _passageiros.Remove(usuario);
        return this;
    }
}
