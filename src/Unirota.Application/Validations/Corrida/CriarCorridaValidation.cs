using FluentValidation;
using Unirota.Application.Commands.Corridas;

namespace Unirota.Application.Validations.Corrida;
public class CriarCorridaValidation : AbstractValidator<CriarCorridaCommand>
{
    public CriarCorridaValidation()
    {
        RuleFor(x => x.Comeco)
            .NotEmpty()
            .WithMessage("Hora de início obrigatório")

            .Must(BeAValidDate)
            .WithMessage("Hora de início inválida");
    }

    private static bool BeAValidDate(DateTime date)
    {
        return date != default;
    }
}
