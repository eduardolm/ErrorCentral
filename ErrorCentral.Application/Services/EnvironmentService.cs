using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class EnvironmentService : GenericService<Environment>, IEnvironmentService
    {
        public EnvironmentService(IGenericRepository<Environment> repository, IValidator<Environment> validator) : base(repository, validator)
        {
        }
    }
}