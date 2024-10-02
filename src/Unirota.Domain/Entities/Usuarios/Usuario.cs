using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Domain.Entities.Usuarios;

public class Usuario : BaseEntity, IAggregateRoot
{
    public string Nome { get; protected set; }
    public string Email { get; protected set; }
    public string? Habilitacao { get; protected set; }
    public string Senha { get; protected set; }
    public string CPF { get; protected set; }
    public DateTime DataNascimento { get; protected set; }
    public string? ImagemUrl { get; protected set; }
    public ICollection<UsuariosGrupo> GruposComoPassageiro { get; private set; } = [];

    public ICollection<Grupo> GruposComoMotorista { get; private set; } = [];


    public Usuario()
    {

    }

    public Usuario(string nome, string email, string senha, string cpf, DateTime dataNascimento)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        CPF = cpf;
        DataNascimento = dataNascimento;
    }

    public Usuario AlterarNome(string nome)
    {
        Nome = nome;
        return this;
    }
    public Usuario AlterarSenha(string senha)
    {
        Senha = senha;
        return this;
    }

    public Usuario AlterarImagem(string imagemUrl)
    {
        ImagemUrl = imagemUrl;
        return this;
    }

    public Usuario AlterarDataNascimento(DateTime dataNascimento)
    {
        DataNascimento = dataNascimento;
        return this;
    }

    public Usuario AdicionarGrupo(Grupo grupo)
    {
        GruposComoMotorista.Add(grupo);
        return this;
    }
}
