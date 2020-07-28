using ErrorCentral.Domain.Interfaces;

namespace ErrorCentral.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}