using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Tests.Unit.Domain
{
    public class EnvironmentIdComparer : IEqualityComparer<Environment>
    {
        public bool Equals(Environment x, Environment y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Environment obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}