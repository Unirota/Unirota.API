using FluentValidation;
using Unirota.Application.Commands.Grupos;

namespace Unirota.Application.Validations.Grupos;

public class CriarGrupoValidation : AbstractValidator<CriarGrupoCommand>
{
    public CriarGrupoValidation()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório");

        RuleFor(x => x.PassageiroLimite)
            .LessThanOrEqualTo(4)
            .WithMessage("Não é possível criar um grupo com mais de 4 passageiros");

        RuleFor(x => x.HoraInicio)
            .Must(BeAValidDate)
            .WithMessage("Hora de início inválida");
    }

    private static bool BeAValidDate(DateTime date)
    {
        return date != default;
    }
}
