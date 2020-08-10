using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Test.Unit.Domain
{
    public class LayerIdComparer : IEqualityComparer<Layer>
    {
        public bool Equals(Layer x, Layer y)
        {
            return x != null && x.Id == y.Id;
        }

        public int GetHashCode(Layer obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}