using Unirota.Domain.Common.Contracts;

namespace Unirota.Domain.Entities.Usuarios;

public class Usuario : BaseEntity, IAggregateRoot
{
    public string Nome { get; protected set; }
    public string Email { get; protected set; }
    public string Habilitacao { get; protected set; }
    public string Senha { get; protected set; }
    public string CPF { get; protected set; }

    public Usuario(string nome, string email, string habilitacao, string senha, string cpf)
    {
        Nome = nome;
        Email = email;
        Habilitacao = habilitacao;
        Senha = senha;
        CPF = cpf;
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
}
