using FluentValidation;
using Unirota.Application.Commands.Veiculos;

namespace Unirota.Application.Validations.Veiculos;

public class CriarVeiculoValidation : AbstractValidator<CriarVeiculosCommand>
{
    public CriarVeiculoValidation()
    {
        RuleFor(x => x.Placa)
           .NotEmpty()
           .WithMessage("Placa é obrigatório");

        RuleFor(x => x.Cor)
            .NotEmpty()
            .WithMessage("A cor é obrigatória");


        RuleFor(x => x.Carroceria)
             .NotEmpty()
             .WithMessage("A carroceria é obrigatória");

    }
}
