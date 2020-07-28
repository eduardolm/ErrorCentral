using FluentValidation;
using Environment = ErrorCentral.Domain.Models.Environment;

namespace ErrorCentral.Application.Validators
{
    public class EnvironmentValidator : AbstractValidator<Environment>
    {
        public EnvironmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome não pode ser deixado em branco.")
                .Length(3, 100)
                .WithMessage("Nome precisa ter entre 3 e 100 caracteres.");
        }
    }
}