using ErrorCentral.Domain.Models;
using FluentValidation;

namespace ErrorCentral.Application.Validators
{
    public class LayerValidator : AbstractValidator<Layer>
    {
        public LayerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nome não pode ser deixado em branco.")
                .Length(3, 100)
                .WithMessage("Nome precisa ter entre 3 e 100 caracteres.");
        }
    }
}