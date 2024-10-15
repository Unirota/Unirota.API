using Mapster;
using Unirota.Application.ViewModels.Enderecos;
using Unirota.Domain.Entities.Enderecos;

namespace Unirota.Infrastructure.Mapping;

public static class EnderecoMapSettings
{
    public static void Configure()
    {
        TypeAdapterConfig<Endereco, EnderecoViewModel>
            .NewConfig()
            .Map(dest => dest.CEP, src => src.CEP)
            .Map(dest => dest.Logradouro, src => src.Logradouro)
            .Map(dest => dest.Numero, src => src.Numero)
            .Map(dest => dest.Cidade, src => src.Cidade)
            .Map(dest => dest.Bairro, src => src.Bairro)
            .Map(dest => dest.UF, src => src.UF)
            .Map(dest => dest.Complemento, src => src.Complemento);
    }
}
