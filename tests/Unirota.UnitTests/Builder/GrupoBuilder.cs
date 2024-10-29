using Bogus;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.UnitTests.Builder;

public class GrupoBuilder
{

    private string _nome = "Grupo Padrão";
    private int _limite = 4;
    private DateTime _inicio = DateTime.Now;
    private int _motoristaId = 1;
    private string _destino = "Destino Padrão";

    public Grupo Build()
    {
        return Build(1).First();
    }

    public ICollection<Grupo> Build(int count)
    {
        var faker = new Faker<Grupo>("pt_BR")
            .CustomInstantiator(f =>
            {
                var grupo = new Grupo(_nome, _limite, _inicio, _motoristaId, _destino);
                return grupo;
            });

        return faker.Generate(count);
    }

    public GrupoBuilder WithNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public GrupoBuilder WithLimite(int limite)
    {
        _limite = limite;
        return this;
    }

    public GrupoBuilder WithInicio(DateTime inicio)
    {
        _inicio = inicio;
        return this;
    }

    public GrupoBuilder WithMotoristaId(int motoristaId)
    {
        _motoristaId = motoristaId;
        return this;
    }

    public GrupoBuilder WithDestino(string destino)
    {
        _destino = destino;
        return this;
    }
}
