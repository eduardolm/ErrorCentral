using ErrorCentral.Domain.Models;
using FluentValidation;

namespace ErrorCentral.Application.Validators
{
    public class LogValidator : AbstractValidator<Log>
    {
        public LogValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome não pode ser deixado em branco.")
                .Length(3, 100)
                .WithMessage("Nome precisa ter entre 3 e 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Descrição não pode ser deixada em branco..")
                .Length(6, 400)
                .WithMessage("Descrição deve ter entre 6 e 400 caracteres..");
        }
    }
}