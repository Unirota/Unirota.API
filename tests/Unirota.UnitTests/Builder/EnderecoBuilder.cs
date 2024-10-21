using Bogus;
using Unirota.Domain.Entities.Enderecos;

namespace Unirota.UnitTests.Builder;

public class EnderecoBuilder
{
    private int _usuarioId;

    public Endereco Build()
    {
        return Build(1).First();
    }

    public ICollection<Endereco> Build(int count)
    {
        var faker = new Faker<Endereco>("pt_BR")
            .CustomInstantiator(f =>
            {
                var cep = f.Address.ZipCode();
                var logradouro = f.Address.StreetName();
                var numero = f.Random.Int(1, 1000);
                var cidade = f.Address.City();
                var bairro = f.Address.CityPrefix();
                var uf = f.Address.StreetSuffix();
                var endereco = new Endereco(cep, logradouro, numero, cidade, bairro, uf, _usuarioId);

                return endereco;
            });

        return faker.Generate(count);
    }

    public EnderecoBuilder WithUsuarioId(int usuarioId)
    {
        _usuarioId = usuarioId;
        return this;
    }
}
