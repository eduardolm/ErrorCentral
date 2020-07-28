using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class StatusService : GenericService<Status>, IStatusService
    {
        public StatusService(IGenericRepository<Status> repository, IValidator<Status> validator) : base(repository, validator)
        {
        }
    }
}