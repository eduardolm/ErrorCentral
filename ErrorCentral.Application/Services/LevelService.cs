using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class LevelService : GenericService<Level>, ILevelService
    {
        public LevelService(IGenericRepository<Level> repository, IValidator<Level> validator) : base(repository, validator)
        {
        }
    }
}