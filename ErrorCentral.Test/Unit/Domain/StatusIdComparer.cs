using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Tests.Unit.Domain
{
    public class StatusIdComparer : IEqualityComparer<Status>
    {
        public bool Equals(Status x, Status y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Status obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}