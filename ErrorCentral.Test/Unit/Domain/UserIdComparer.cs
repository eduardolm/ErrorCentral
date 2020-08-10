using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Test.Unit.Domain
{
    public class UserIdComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}