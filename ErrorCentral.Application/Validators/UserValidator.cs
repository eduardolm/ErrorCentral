using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using FluentValidation;

namespace ErrorCentral.Application.Validators
{
     public class UserValidator : AbstractValidator<User>
     {
         private readonly MainContext _context;
        public UserValidator(MainContext context)
        {
            _context = context;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Nome não pode ser deixado em branco..")
                .Length(3, 100)
                .WithMessage("O nome deve ter entre 3 e 100 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("E-mail deve ser um endereço válido.")
                .NotEmpty()
                .WithMessage("E-mail não pode ser deixado em branco.")
                .Length(6, 100)
                .WithMessage("E-mail deve ter entre 6 e 100 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("O campo de senha não pdoe ser deixado em branco.")
                .Length(3, 100)
                .WithMessage("A senha deve possuir até 100 caracteres.");

            RuleFor(x => x)
                .Must(x => !IsDuplicate(x))
                .WithMessage("Usuário já cadastrado.");
        }

        private bool IsDuplicate(User user)
        {
            var compUser = (from u in _context.User.ToList() select u);
            return compUser.Any(x => x.FullName == user.FullName &&
                                     x.Email == user.Email);
        }
    }
}