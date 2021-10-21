using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQSamples.RepositoryClasses
{
    public static class ProductHelper
    {
      public static IEnumerable<Product> ByColor(this IEnumerable<Product> query, string color)
        {
            return query.Where(prod => prod.Color == color);
        }
    }
}
