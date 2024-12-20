﻿using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Domain.Entities.Enderecos;

public class Endereco : BaseEntity, IAggregateRoot
{

    public string CEP { get; private set; }
    public string Logradouro { get; private set; }
    public int Numero { get; private set; }
    public string Cidade { get; private set; }
    public string Bairro { get; private set; }
    public string UF { get; private set; }
    public string? Complemento { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; }

    public Endereco()
    {

    }

    public Endereco(string cep, string logradouro, int numero, string cidade, string bairro, string uf, int usuarioId)
    {
        CEP = cep;
        Logradouro = logradouro;
        Numero = numero;
        Cidade = cidade;
        Bairro = bairro;
        UF = uf;
        UsuarioId = usuarioId;
    }

    public Endereco AlterarComplemento(string? complemento)
    {
        Complemento = complemento;
        return this;
    }

    public Endereco AlterarCEP(string cep)
    {
        CEP = cep;
        return this;
    }

    public Endereco AlterarLogradouro(string logradouro)
    {
        Logradouro = logradouro;
        return this;
    }

    public Endereco AlterarNumero(int numero)
    {
        Numero = numero;
        return this;
    }

    public Endereco AlterarCidade(string cidade)
    {
        Cidade = cidade;
        return this;
    }

    public Endereco AlterarBairro(string bairro)
    {
        Bairro = bairro;
        return this;
    }

    public Endereco AlterarUF(string uf)
    {
        UF = uf;
        return this;
    }
}
