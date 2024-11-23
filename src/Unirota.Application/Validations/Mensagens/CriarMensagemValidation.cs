using FluentValidation;
using Unirota.Application.Commands.Mensagens;

namespace Unirota.Application.Validations.Mensagens;

public class CriarMensagemValidation : AbstractValidator<CriarMensagemCommand>
{
    public CriarMensagemValidation()
    {
        RuleFor(x => x.Conteudo)
            .NotEmpty()
            .MaximumLength(512)
            .WithMessage("A mensagem não pode ter mais de 512 caracteres");
    }

}
