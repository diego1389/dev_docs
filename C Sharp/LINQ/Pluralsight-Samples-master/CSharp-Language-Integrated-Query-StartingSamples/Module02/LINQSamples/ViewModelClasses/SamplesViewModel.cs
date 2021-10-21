using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQSamples.RepositoryClasses;

namespace LINQSamples
{
  public class SamplesViewModel
  {
    #region Constructor
    public SamplesViewModel()
    {
      // Load all Product Data
      Products = ProductRepository.GetAll();
    }
    #endregion

    #region Properties
    public bool UseQuerySyntax { get; set; }
    public List<Product> Products { get; set; }
    public string ResultText { get; set; }
    #endregion

    #region GetAllLooping
    /// <summary>
    /// Put all products into a collection via looping, no LINQ
    /// </summary>
    public void GetAllLooping()
    {
      List<Product> list = new List<Product>();
        foreach (var item in Products)
        {
                list.Add(item);
        }
     
      ResultText = $"Total Products: {list.Count}";
    }
    #endregion

    #region GetAll
    /// <summary>
    /// Put all products into a collection using LINQ
    /// </summary>
    public void GetAll()
    {
      List<Product> list = new List<Product>();

      if (UseQuerySyntax) {
        // Query Syntax
        list = (from prod in Products select prod).ToList();
        
      }
      else {
        // Method Syntax
        list = Products.Select(prod => prod).ToList();
      }

      ResultText = $"Total Products: {list.Count}";
    }
    #endregion

    #region GetSingleColumn
    /// <summary>
    /// Select a single column
    /// </summary>
    public void GetSingleColumn()
    {
      StringBuilder sb = new StringBuilder(1024);
      List<string> list = new List<string>();

      if (UseQuerySyntax) {
        // Query Syntax
        list.AddRange(from prod in Products select prod.Name);
        
      }
      else {
        // Method Syntax
        list.AddRange(Products.Select(prod => prod.Name));
        
      }

      foreach (string item in list) {
        sb.AppendLine(item);
      }

      ResultText = $"Total Products: {list.Count}" + Environment.NewLine + sb.ToString();
      Products.Clear();
    }
    #endregion

    #region GetSpecificColumns
    /// <summary>
    /// Select a few specific properties from products and create new Product objects
    /// </summary>
    public void GetSpecificColumns()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            select new Product
                            {
                                ProductID = prod.ProductID,
                                Name = prod.Name,
                                Size = prod.Size
                            }).ToList();
       
      }
      else {
                // Method Syntax
                Products = Products.Select(prod => new Product
                {
                    ProductID = prod.ProductID,
                    Name = prod.Name,
                    Size = prod.Size
                }).ToList();
       
      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region AnonymousClass
    /// <summary>
    /// Create an anonymous class from selected product properties
    /// </summary>
    public void AnonymousClass()
    {
      StringBuilder sb = new StringBuilder(2048);

      if (UseQuerySyntax) {
                // Query Syntax
                var products = (from prod in Products
                                select new
                                {
                                    Identifier = prod.ProductID,
                                    ProductName = prod.Name,
                                    ProductSize = prod.Size
                                }).ToList();
        // Loop through anonymous class
        foreach (var prod in products) {
          sb.AppendLine($"Product ID: {prod.Identifier}");
          sb.AppendLine($"   Product Name: {prod.ProductName}");
          sb.AppendLine($"   Product Size: {prod.ProductSize}");
        }
      }
      else {
                // Method Syntax
                var products = Products.Select(prod => new
                {
                    Identifier = prod.ProductID,
                    ProductName = prod.Name,
                    ProductSize = prod.Size
                }).ToList();
        
        // Loop through anonymous class
        foreach (var prod in products) {
          sb.AppendLine($"Product ID: {prod.Identifier}");
          sb.AppendLine($"   Product Name: {prod.ProductName}");
          sb.AppendLine($"   Product Size: {prod.ProductSize}");
        }
      }

      ResultText = sb.ToString();
      Products.Clear();
    }
    #endregion

    #region OrderBy
    /// <summary>
    /// Order products by Name
    /// </summary>
    public void OrderBy()
    {
      if (UseQuerySyntax) {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Name
                            select prod).ToList();

      }
      else {
                // Method Syntax
                Products = Products.OrderBy(prod => prod.Name).ToList();

      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region OrderByDescending Method
    /// <summary>
    /// Order products by name in descending order
    /// </summary>
    public void OrderByDescending()
    {
            if (UseQuerySyntax)
            {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Color descending, prod.Name
                            select prod).ToList();

            }
            else
            {
                // Method Syntax
                Products = Products.OrderByDescending(prod => prod.Name)
                    .ThenBy(prod => prod.Name)
                    .ToList();

            }

            ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region OrderByTwoFields Method
    /// <summary>
    /// Order products by Color descending, then Name
    /// </summary>
    public void OrderByTwoFields()
    {
            if (UseQuerySyntax)
            {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Color descending, prod.Name
                            select prod).ToList();

            }
            else
            {
                // Method Syntax
                Products = Products.OrderByDescending(prod => prod.Name)
                    .ThenBy(prod => prod.Name)
                    .ToList();

            }

            ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    public void WhereExpression()
        {
            string search = "L";
            if (UseQuerySyntax)
            {
                Products = (from prod in Products
                            where prod.Name.StartsWith(search)
                            select prod).ToList();
            }
            else
            {
                Products = Products.Where(prod => prod.Name.StartsWith(search)).ToList();
            }
            ResultText = $"Total Products: {Products.Count}";
        }
        public void WhereTwoFields()
        {
            string search = "L";
            decimal cost = 100;
            if (UseQuerySyntax)
            {
                Products = (from prod in Products
                            where prod.Name.StartsWith(search) && prod.StandardCost > cost
                            select prod).ToList();
            }
            else
            {
                Products = Products.Where(prod => prod.Name.StartsWith(search)
                && prod.StandardCost > cost).ToList();
            }
            ResultText = $"Total Products: {Products.Count}";
        }

        public void WhereExtensionMethod()
        {
            string search = "Red";

            if (UseQuerySyntax)
            {
                Products = (from prod in Products select prod).ByColor(search).ToList();
            }
            else
            {
                Products = Products.Select(prod => prod).ByColor(search).ToList();
            }
            ResultText = $"Total Products: {Products.Count}";
        }

        public void First()
        {
            string search = "Red";
            Product value;
            try
            {
                if (UseQuerySyntax)
                {
                    //Query syntax
                    value = (from prod in Products
                             select prod).First(prod => prod.Color == search);
                }
                else
                {
                    //Method syntax
                    value = Products.First(prod => prod.Color == search);
                }
                ResultText = $"Found {value}";
            }
            catch (Exception ex)
            {
                ResultText = $"Not found";
            }
        }

        public void FirstOrDefault()
        {
            string search = "Red";
            Product value;

            if (UseQuerySyntax)
            {
                //Query syntax
                value = (from prod in Products
                         select prod).FirstOrDefault(prod => prod.Color == search);
            }
            else
            {
                //Method syntax
                value = Products.FirstOrDefault(prod => prod.Color == search);
            }

            if(value == null)
            {
                ResultText = $"Not found";
            }
            else
            {
                ResultText = $"Found {value}";
            }
        }

        public void Last()
        {
            string search = "Red";
            Product value;
            try
            {
                if (UseQuerySyntax)
                {
                    //Query syntax
                    value = (from prod in Products
                             select prod).Last(prod => prod.Color == search);
                }
                else
                {
                    //Method syntax
                    value = Products.Last(prod => prod.Color == search);
                }
                ResultText = $"Found {value}";
            }
            catch (Exception ex)
            {
                ResultText = $"Not found";
            }
        }

        public void LastOrDefault()
        {
            string search = "Red";
            Product value;

            if (UseQuerySyntax)
            {
                //Query syntax
                value = (from prod in Products
                         select prod).LastOrDefault(prod => prod.Color == search);
            }
            else
            {
                //Method syntax
                value = Products.LastOrDefault(prod => prod.Color == search);
            }

            if (value == null)
            {
                ResultText = $"Not found";
            }
            else
            {
                ResultText = $"Found {value}";
            }
        }

        public void Single()
        {
            int search = 706;
            Product value;
            try
            {
                if (UseQuerySyntax)
                {
                    //Query syntax
                    value = (from prod in Products
                             select prod).Single(prod => prod.ProductID == search);
                }
                else
                {
                    //Method syntax
                    value = Products.Single(prod => prod.ProductID == search);
                }
                ResultText = $"Found {value}";
            }
            catch (Exception ex)
            {
                ResultText = $"Not found";
            }
        }

        public void SingleOrDefault ()
        {
            int search = 706;
            Product value;

            if (UseQuerySyntax)
            {
                //Query syntax
                value = (from prod in Products
                         select prod).SingleOrDefault(prod => prod.ProductID == search);
            }
            else
            {
                //Method syntax
                value = Products.SingleOrDefault(prod => prod.ProductID == search);
            }

            if (value == null)
            {
                ResultText = $"Not found";
            }
            else
            {
                ResultText = $"Found {value}";
            }
        }
    }
}
