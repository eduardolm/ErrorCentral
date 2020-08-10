using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Test.Unit.Domain
{
    public class LogIdComparer : IEqualityComparer<Log>
    {
        public bool Equals(Log x, Log y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(Log obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}