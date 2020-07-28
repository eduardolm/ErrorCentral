using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Tests.Unit.Domain
{
    public class LayerIdComparer : IEqualityComparer<Layer>
    {
        public bool Equals(Layer x, Layer y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Layer obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}