using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class LayerService : GenericService<Layer>, ILayerService
    {
        public LayerService(IGenericRepository<Layer> repository, IValidator<Layer> validator) : base(repository, validator)
        {
        }
    }
}