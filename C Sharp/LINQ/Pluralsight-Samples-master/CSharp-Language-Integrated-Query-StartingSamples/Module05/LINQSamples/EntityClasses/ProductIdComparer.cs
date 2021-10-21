using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LINQSamples.EntityClasses
{
    public class ProductIdComparer : EqualityComparer<Product>
    {
        public override bool Equals(Product x, Product y)
        {
            return (x.ProductID == y.ProductID);
        }

        public override int GetHashCode([DisallowNull] Product obj)
        {
            return obj.ProductID.GetHashCode();
        }
    }
}
