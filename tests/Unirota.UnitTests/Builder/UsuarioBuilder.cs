﻿using Bogus;
using Unirota.Domain.Entities.Enderecos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.UnitTests.Builder;

public class UsuarioBuilder
{
    private string _cpf;
    private Endereco _endereco;
    private string _habilitacao;

    public Usuario Build()
    {
        return Build(1).First();
    }

    public ICollection<Usuario> Build(int count)
    {
        var faker = new Faker<Usuario>("pt_BR")
                .CustomInstantiator(f =>
                {
                    var nome = f.Name.FullName();
                    var email = f.Internet.Email(nome);
                    var senha = f.Internet.Password();
                    var dataNascimento = f.Date.Recent(365);
                    
                    var usuario = new Usuario(nome, email, senha, _cpf, dataNascimento);
                    var endereco = new EnderecoBuilder().WithUsuarioId(usuario.Id).Build();

                    usuario.AlterarEndereco(_endereco ?? endereco);
                    usuario.AlterarHabilitacao(_habilitacao ?? "");

                    return usuario;
                });

        return faker.Generate(count);
    }

    public UsuarioBuilder WithCPF(string cpf)
    {
        _cpf = cpf;
        return this;
    }

    public UsuarioBuilder WithEndereco(Endereco endereco)
    {
        _endereco = endereco;
        return this;
    }

    public UsuarioBuilder WithHabilitacao(string habilitacao)
    {
        _habilitacao = habilitacao;
        return this;
    }
}
