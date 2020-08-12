using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Test.Unit.Domain
{
    public class EnvironmentIdComparer : IEqualityComparer<Environment>
    {
        public bool Equals(Environment x, Environment y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(Environment obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}