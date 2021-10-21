using System;
using System.Collections.Generic;
using System.Linq;
using LINQSamples.EntityClasses;

namespace LINQSamples
{
  public class SamplesViewModel
  {
    #region Constructor
    public SamplesViewModel()
    {
      // Load all Product Data
      Products = ProductRepository.GetAll();
      // Load all Sales Data
      Sales = SalesOrderDetailRepository.GetAll();
    }
    #endregion

    #region Properties
    public bool UseQuerySyntax { get; set; } = true;
    public List<Product> Products { get; set; }
    public List<SalesOrderDetail> Sales { get; set; }
    public string ResultText { get; set; }
    #endregion

    #region ForEach Method
    /// <summary>
    /// ForEach allows you to iterate over a collection to perform assignments within each object.
    /// In this sample, assign the Length of the Name property to a property called NameLength
    /// When using the Query syntax, assign the result to a temporary variable.
    /// </summary>
    public void ForEach()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            let tmp = prod.NameLength = prod.Name.Length
                            select prod).ToList();

      }
      else {
                // Method Syntax
                Products.ForEach(prod => prod.NameLength = prod.Name.Length);

      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region ForEachCallingMethod Method
    /// <summary>
    /// Iterate over each object in the collection and call a method to set a property
    /// This method passes in each Product object into the SalesForProduct() method
    /// In the SalesForProduct() method, the total sales for each Product is calculated
    /// The total is placed into each Product objects' ResultText property
    /// </summary>
    public void ForEachCallingMethod()
    {
      if (UseQuerySyntax) {
                Products = (from prod in Products
                            let tmp = prod.TotalSales = SalesForProduct(prod)
                            select prod).ToList();

            }
      else {
        // Method Syntax
            Products.ForEach(prod => prod.TotalSales = SalesForProduct(prod));

      }

      ResultText = $"Total Products: {Products.Count}";
    }

    /// <summary>
    /// Helper method called by LINQ to sum sales for a product
    /// </summary>
    /// <param name="prod">A product</param>
    /// <returns>Total Sales for Product</returns>
    private decimal SalesForProduct(Product prod)
    {      
      return Sales.Where(sale => sale.ProductID == prod.ProductID)
                  .Sum(sale => sale.LineTotal);
    }
    #endregion

    #region Take Method
    /// <summary>
    /// Use Take() to select a specified number of items from the beginning of a collection
    /// </summary>
    public void Take()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Name
                            select prod).Take(5).ToList();

      }
      else {
                // Method Syntax
                Products = Products.OrderBy(prod => prod.Name).Take(5).ToList();
      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region TakeWhile Method
    /// <summary>
    /// Use TakeWhile() to select a specified number of items from the beginning of a collection based on a true condition
    /// </summary>
    public void TakeWhile()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Name
                            select prod).TakeWhile(prod => prod.Name.StartsWith("A")).ToList();

            }
      else {
        // Method Syntax
             Products = Products.OrderBy(prod => prod.Name).TakeWhile(prod => prod.Name.StartsWith("A")).ToList();
            }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region Skip Method
    /// <summary>
    /// Use Skip() to move past a specified number of items from the beginning of a collection
    /// </summary>
    public void Skip()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Name
                            select prod).Skip(20).ToList();
            }
      else {
                // Method Syntax
                Products = Products.OrderBy(prod => prod.Name).Skip(20).ToList();
            }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region SkipWhile Method
    /// <summary>
    /// Use SkipWhile() to move past a specified number of items from the beginning of a collection based on a true condition
    /// </summary>
    public void SkipWhile()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Name
                            select prod).SkipWhile(prod => prod.Name.StartsWith("A")).ToList();

            }
      else {
                // Method Syntax
                Products = Products.OrderBy(prod => prod.Name).SkipWhile(prod => prod.Name.StartsWith("A")).ToList();

            }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region Distinct
    /// <summary>
    /// The Distinct() operator finds all unique values within a collection
    /// In this sample you put distinct product colors into another collection using LINQ
    /// </summary>
    public void Distinct()
    {
      List<string> colors = new List<string>();

      if (UseQuerySyntax) {
                // Query Syntax
                colors = (from prod in Products
                          select prod.Color).Distinct().ToList();

      }
      else {
                // Method Syntax
                colors = Products.Select(prod => prod.Color).Distinct().ToList();
      }

      // Build string of Distinct Colors
      foreach (var color in colors) {
        Console.WriteLine($"Color: {color}");
      }
      Console.WriteLine($"Total Colors: {colors.Count}");

      // Clear products
      Products.Clear();
    }
    #endregion
        public void All()
        {
            string search = " ";
            bool value;
            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).All(prod => prod.Name.Contains(search));
            }
            else
            {
                value = Products.All(prod => prod.Name.Contains(search));
            }
            ResultText = $"Do all name properties contain a '{search}'? {value}";
        }
        public void Any()
        {
            string search = "z";
            bool value;
            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).Any(prod => prod.Name.Contains(search));
            }
            else
            {
                value = Products.Any(prod => prod.Name.Contains(search));
            }
            ResultText = $"Do any name properties contain a '{search}'? {value}";
        }

        public void LINQContains()
        {
            bool value;
            List<int> numbers = new List<int>() { 1, 2, 3, 4, 5 };
            if (UseQuerySyntax)
            {
                value = (from num in numbers select num).Contains(3);
            }   
            else
            {
                value = numbers.Contains(3);
            }
            ResultText = $"Is the number in the collection? {value}";
        }

        public void LINQContainsUsingComparer()
        {
            int search = 744;
            bool value;
            ProductIdComparer pc = new ProductIdComparer();
            Product prodToFind = new Product { ProductID = search };

            if (UseQuerySyntax)
            {
                value = (from prod in Products
                         select prod).Contains(prodToFind, pc); 
            }
            else
            {
                value = Products.Contains(prodToFind, pc);
            }
            ResultText = $"Procut ID: {search} is in Products collection? {value}";
        }
    }
}
