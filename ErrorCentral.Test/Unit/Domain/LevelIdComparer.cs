using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Test.Unit.Domain
{
    public class LevelIdComparer : IEqualityComparer<Level>
    {
        public bool Equals(Level x, Level y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Level obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}