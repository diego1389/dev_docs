- Query any type of collection as long as it implements IEnumerable<T>.
- Query external datasources (xml, databases, JSON, csv, etc).
- Unified approach for querying any type of objects.
- Helps eliminate loops.

|SQL |LINQ   |
|---|---|
|SELECT Name FROM Products WHERE ListPrice > 10   | from prod in Products where prod.ListPrice > 10 select prod.Name   |
Two LINQ syntaxis

|Query |Method   |
|---|---|
| from prod in Products where prod.ListPrice > 10 select prod.Name  |Products.Where(prod => prod.ListPrice > 10).Select(prod => prod.Name)    |

- LINQ operations: Select, Projection (change shape), Order, Get an element, filter, iteration / partitioning, quantify, set comparison, set operations (uion, concat), joining, grouping, distinct sets, aggregation.

## Using LINQ select and order operations

- We can use LINQ with reflection, strings, file and directories, entities, and datasets.
    ```c#
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
    ```
- Get a single column:
    ```c#
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
    ```
- Get specific columns:
    ```c#
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
    ```
- Anonymous classes:
    ```c#
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
    ```
- Ordering data:
    ```c#
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
    ```
- Ordering data in descending order
    ```c#
    public void OrderByDescending()
    {
            if (UseQuerySyntax)
            {
                // Query Syntax
                Products = (from prod in Products
                            orderby prod.Name descending
                            select prod).ToList();

            }
            else
            {
                // Method Syntax
                Products = Products.OrderByDescending(prod => prod.Name).ToList();

            }

            ResultText = $"Total Products: {Products.Count}";
    }
    ```
- Ordering using multiple fields (there is also ThenByDescending)
    ```c#
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
    ```
## Extract data using filtering and element operations

- Filter data using a where expression
    ```c#
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
    ```
- Using a Where Expression with Two Fields
    ```c#
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
    ```
- Using custom extension methods. For extension methods must always be prefixed with the this keyword to specify against what type this extension method should be applied. 
    ```c#
    public static class ProductHelper
    {
      public static IEnumerable<Product> ByColor(this IEnumerable<Product> query, string color)
        {
            return query.Where(prod => prod.Color == color);
        }
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
    ```
- Select a single item:
    ```c#
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
    ```
- Select the last item:
    ```c#
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
    ```
- Find a single item:
    - Single throws an exception if it didn't find the item or it found mora than one.
    ```c#
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
    ```

    ## Extract distinct values, assign values and partition collections

- Set operations: iterate over entire collection, set a property value in collection.  
- Assigning values to properties using foreach:
    ```c#
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
    ```
- Calling a method to set a property
    ```c#
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
    ```
- Get a Specific Amount of Items Using Take()
    ```c#
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
    ```
- Using TakeWhile() to select a certain amount of elements while condition is true:
    ```c#
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

    }
    else {
        // Method Syntax

    }

    ResultText = $"Total Products: {Products.Count}";
    }
    ```
- Using Skip() and SkipWhile() to pass over items
    ```c#
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
    ```
- Getting Unique Values from a Collection using Distinct()
    ```c#
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
    ```
- ## Identify what kind of data is contained in collections

- Using All() to See if All Items Match a Condition
    ```c#
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
    ```
- Using Any() to see if Any() items match a condition. 
    ```c#
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
    ```
- Using Contains() with an Integer List of Items
    - It only works for simple data type collections (int, decimal, string, etc).
    ```c#
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
    ```
- EqualityComparer:
    ```c#
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
    ```
- ## Compare and union two collections

- SequenceEqual: 
    - Compares two collections for equality.
    - Simple data types checks values.
    - Object data types checks references. 
    ```c#
    bool value;
    List<int> list1 = new List<int>{1,2,3,4,5};
    List<int> list2 = new List<int>{1,2,3,4,5};

    //Querysyntax
    value = (from num in list1 select num).SequenceEqual(list2);
    //Method syntax
    value = list1.SequenceEqual(list2);
    ```
    - To use SequenceEqual with objects you need a comparer just as with Contains.
    ```c#
    //Querysyntax
    value = (from prod in list1 select prod).SequenceEqual(list2, pc);
    //Method syntax
    value = list1.SequenceEqual(list2, pc);
    ```
- Except: find all values that are in one list but not in the other:
    ```c#
    List<int> exceptions;
    List<int> list1 = new List<int>{1,2,3,4,5};
    List<int> list2 = new List<int>{3,4,5};

    //Querysyntax
    exceptions = (from num in list1 select num).Except(list2).ToList(); //1 2
    //Method syntax
    exceptions = list1.Except(list2).ToList(); //1 2 
    ```
    - Except also need a comparer when used with objects.
- Intersect
    - Get common elements that exist in both lists
    ```c#
    //Querysyntax
    Products = (from prod in list1 select prod).Intersect(list2, pc).ToList();
    //Method syntax
    Products = list1.Intersect(list2, pc).ToList();
    ```
    ```c#
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    namespace LinqComparer
    {
        public static class Program
        {
            public static void Main(string[] args)
            {
                var pc = new ProductIdComparer();

                var products1 = new List<Product>
                {
                    new(){Name = "Apple", Id = 1 },
                    new(){Name = "Bee", Id = 2 },
                    new(){Name = "Carrot", Id = 3 },
                };

                var products2 = new List<Product>
                {
                    new(){Name = "Bee", Id = 2 },
                    new(){Name = "Carrot", Id = 3 },
                    new(){Name = "Deviled Eggs", Id = 4 },
                };

                var commonProducts = products1.Intersect(products2, pc);
                foreach (var prod in commonProducts)
                {
                    Console.WriteLine(prod);
                }
            }
        }

        public class Product
        {
            public string Name { get; set; }
            public int Id { get; set; }

            public override string? ToString()
            {
                return $"Name: {Name} Id: {Id}";
            }
        }

        public class ProductIdComparer : EqualityComparer<Product>
        {
            public override bool Equals(Product x, Product y)
            {
                return (x.Id == y.Id);
            }

            public override int GetHashCode([DisallowNull] Product obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
    ```
- Union and Concat add the contents of two lists together. Union() checks for duplicates and Concat() does not. 
    ```c#
    //Querysyntax
    Products = (from prod in list1 select prod).Union(list2, pc).ToList();
    Products = (from prod in list1 select prod).Concat(list2).ToList();
    //Method syntax
    Products = list1.Union(list2, pc).ToList();
    Products = list1.Union(list2).ToList();
    ```
 ## Joining two collections together

 - Inner join:
    ```c#
    //Query syntax
    var query = (from prod in Products join sale in Sales
                on prod.ProductID equals sale.ProductID
                select new {
                    prod.ProductID,
                    sale.LineTotal});
    foreach(var item in query){
        //item.LineTotal
    }
    //Method syntax
    var query = Products.Join(Sales, prod => prod.ProductID, sale => sale.ProductID,
    (prod, sale)=> new {
        prod.ProductID,
        sale.LineTotal
    });
    ```
- Inner join with two fields:
    ```c#
    short qty = 6;
        //Query syntax
    var query = (from prod in Products join sale in Sales
                on new {prod.ProductID, Qty = qty} 
                equals {sale.ProductID, Qty = sale.OrderQty}
                select new {
                    prod.ProductID,
                    sale.LineTotal});
    foreach(var item in query){
        //item.LineTotal
    }
    ```
- Group join: one to many relation.
    ```c#
    public partial class ProductSales{
        public Product Product{get; set;}
        public IEnumerable<SalesOrderDetail> Sales {get; set;}
    }
    IEnumerable<ProductSales> grouped;
    //Query syntax
    grouped = (from prod in Products
                join sale in Sales
                on prod.ProductID equals sale.ProductID
                into sales //new variable
                select new ProductSales{
                    Product = prod,
                    Sales = sales //one-to-many
                });
    //Method syntax
    grouped = Products.GroupJoin(Sales, prod => prod.ProductID,
                                        sale => sale.ProductID,
                                        (prod, sales)=> new ProductSales{
                                            Product = prod,
                                            Sales = sales.ToList();
                                        });
    ```
- Left outer join
    ```c#
    /*Query syntax*/
    var query = (from prod in Products
                join sale in Sales
                on prod.ProductID equals sale.ProductID
                into sales //new variable
                from sale in sales.DefaultIfEmpty()
                select new {
                    prod.ProductID,
                    prod.Name,
                    sale?.SalesOrderId,
                    sale?.LineTotal
                }).OrderBy(ps => ps.Name);
    //Method syntax
    var query = Products.SelectMany(sale => Sales.Where(s=>sale.ProductID == s.ProductID)
                        .DefaultIfEmpty(), 
                        (prod, sale)=> new{
                            prod.ProductID,
                            sale?.SalesOrderID
                        }).OrderBy(ps => ps.Name);

    ```
 ## Creating a left outer join using Groupby

```c#
IEnumerable<IGrouping<string, Product>> sizeGroup;
//Query syntax
sizeGroup = (from prod in Products
            orderby prod.Size
            group prod by prod.Size);
//MEthod syntax
sizeGroup = Products.OrderBy(prod => prod.Size)
                    .GroupBy(prod => prod.Size);

foreach(var group in sizeGroup){
    Console.WriteLine($"Key: {group.Key} Count: {group.Count}");
    foreach(var prod in group){
        Console.WriteLine($"ProductID: {prod.ProductID}"); //etc
    }
}
```
- Group by using into a select
    ```c#
    IEnumerable<IGrouping<string, Product>> sizeGroup;
    //Query syntax
    sizeGroup = (from prod in Products
                orderby prod.Size
                group prod by prod.Size into sizes
                select sizes);
    //MEthod syntax
    sizeGroup = Products.OrderBy(prod => prod.Size)
                        .GroupBy(prod => prod.Size);

    foreach(var group in sizeGroup){
        Console.WriteLine($"Key: {group.Key} Count: {group.Count}");
        foreach(var prod in group){
            Console.WriteLine($"ProductID: {prod.ProductID}"); //etc
        }
    }
    ```
- If you want to order by the key:
    ```c#
    IEnumerable<IGrouping<string, Product>> sizeGroup;
    //Query syntax
    sizeGroup = (from prod in Products
                group prod by prod.Size into sizes
                orderby sizes.Key
                select sizes);
        //MEthod syntax
        sizeGroup = Products.GroupBy(prod => prod.Size)
                            .OrderBy(sizes => sizes.Key)
                            .Select(sizes => sizes);

    foreach(var group in sizeGroup){
        Console.WriteLine($"Key: {group.Key} Count: {group.Count}");
        foreach(var prod in group){
            Console.WriteLine($"ProductID: {prod.ProductID}"); //etc
        }
    }
    ```
- Filtering grouped results:
    ```c#
    IEnumerable<IGrouping<string, Product>> sizeGroup;
    //Query syntax
    sizeGroup = (from prod in Products
                group prod by prod.Size into sizes
                where sizes.Count() > 2
                select sizes);
    //MEthod syntax
    sizeGroup = Products.GroupBy(prod => prod.Size)
                        .Where(sizes => sizes.Count() > 2)
                        .Select(sizes => sizes);

    foreach(var group in sizeGroup){
        Console.WriteLine($"Key: {group.Key} Count: {group.Count}");
        foreach(var prod in group){
            Console.WriteLine($"ProductID: {prod.ProductID}"); //etc
        }
    }
    ```
- Creating a subquery:
    ```c#
    public partial class SaleProducts{
        public int SalesOrderId{get; set;}
        public List<Product> Products{get; set;}
    }

    IEnumerable<SaleProducts> salesGroup;
    //QuerySyntax
    salesGroup = (from sale in Sales
                group sale by sale.SalesOrderID
                into sales
                select new SaleProduct{
                    SalesOrderId = sales.Key,
                    Products = (from prod in Products
                    join sale in Sales
                    on prod.ProductID equals sale.ProductID
                    where sale.SalesOrderId == sales.Key
                    select prod).ToList()
                })
    //Method syntax
    salesGroup = Sales.GroupBy(sale => sale.SalesOrderId)
                    .Select(sales => new SaleProducts{
                        SalesOrderId = sales.Key,
                        Products = Products.Join(
                            sales,
                            prod => prod.ProductID,
                            sale => sale.ProductID,
                            (prod,sale)=> prod).ToList()
                        )
                    });
    ```
## Aggregating data in collections

- Count and Count using a filter:
    ```c#
    int value;
    //Query syntax
    value = (from prod in Products
            select prod).Count();
    //Method syntax
    value = Products.Count();
    ```
    - Count filtered:
    ```c#
    int value;
    string search = "Red";
    //Query syntax
    value = (from prod in Products
            select prod).Count(prod => prod.Color == search);
    /*
    value = (from prod in Products
            where prod.Color == search
            select prod).Count();
    */
    //Method syntax
    value = Products.Count(prod => prod.Color == search);
    ```
- Using Min() and Max()
    ```c#
    decimal? value;
    //Query syntax
    value = (from prod in Products
            select prod.ListPrice).Min(); //Max()
    //or
    /*value = (from prod in Products
            select prod).Min(prod => prod.ListPrice);*/

    //Method syntax
    value = Products.Min(prod => prod.ListPrice);//Max
    ```
- Using Average() and Sum()
    ```c#
    decimal? value;
    //Query syntax
    value = (from prod in Products
            select prod.ListPrice).Average(); //Sum()
    //or
    /*value = (from prod in Products
            select prod).Average(prod => prod.ListPrice);//Sum()*/

    //Method syntax
    value = Products.Average(prod => prod.ListPrice);//Sum
    ```
- Custom calculations:
    - Iterate over collection
    - Supply a custom method for calculating
    - Returns a single value
    ```c#
    decimal value = 0;
    //Query syntax
    value = (from prod in Products select prod)
            .Aggregate(0M, (sum, prod)=>
            {
                sum += prod.ListPrice
            })
    //Method syntax
    value = Products.Aggregate(0M, (sum, prod)=> sum+=prod.ListPrice);
    ```
- Calculating aggregates for groups:
    ```c#
    //Query syntax
    var stats = (from prod in Products
                group prod by prod.Size into sizeGroup
                where sizeGroup.Count() > 0
                select new 
                {
                    Size = sizeGroup.Key,
                    TotalProducts = sizeGroup.Count(),
                    Max = sizeGroup.Max(s => s.ListPrice)
                }
                into result 
                order by result.Size
                select result);
    //Method syntax
    var stats = Products.GroupBy(prod => prod.Size)
                        .Where(sizeGroup => sizeGroup.Count() > 0)
                        .Select(sizeGroup=>new{
                            Size = sizeGroup.Key,
                            TotalProducts = sizeGroup.Count(),
                            Max = sizeGroup.Max(s => s.ListPrice)
                        })
                        .OrderBy(result => result.Size)
                        .Select(result =>  result);
    ```

## Deferred execution

- Types of Linq execution:
    - Deferred:
        - Includes streaming and not streaming.
            - Streaming:
                - Results can be returned prior to the entire collection is read.
                - Distinct, GroupBy, Join, Select, Skip, Take, Union, Where
            - Non streaming: 
                - All data must be read before a result can be returned. 
                - Except, group by, groupjoin, intersect, join, orderby, thenby.
        - A data structure ready to execute. The query is not executed until the value is needed.
            - foreach
            - Count
            - ToList
            - OrderBy, etc. 
    - Immediate:
        - Query is executed immediately.
- Create a filter using streaming execution (similar to what Where() does under the hood).
    ```c#
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate){
        foreach(var item in source){
            if(predicate(item))
                yield return item; //yield returns control every time it finds something (it exits this method). 
        }
    }

    IEnumerable<Product> query;
    query = Products.Filter(prod => prod.Color == "Red");
    ```
- Always use the yield when creating custom extension methods to Linq.
- For better performance first filter the data in one statement and apply order by in another one. 


## Interview questions

1. Linq provides straightforward data access from the in-memory XML documents, databases, objects, etc. 
    - .Net component that links initial data querying abilities to the .NET languages. 
    - Types: 
        1. Linq to XML
        2. Linq to objects
        3. Linq to SQL
        4. Linq to entities
        5. Linq to Dataset
2. Benefits?
    - Fetching complicated queries in the dataset.
    - Combining values from two separate data sets.
    - More functionality than ADO.NET
3. Different methods to write LINQ query syntax?
    - Expression syntax (query syntax).
    - Method extension syntax
4. Define "Let" and  "Where" clauses:
    - Let: define the variable and assign it to the value calculated from data values.
    - Where: enables filters to the query.
5. LINQ over stored procedure?
    - Benefits: 
        - Deployment is easier (compiled to DLL).
        - Debugging (not easy to debug an SP).
        - Type safety. 
    - Draw-backs:
        - SP is faster than Linq
6. Linq standard query operators?
    - Techniques that make the linq pattern. They offer query capabilities for the projection, aggregation, filtering, sorting, etc.
    - One grop works on the IEnumerable<T> and other work on IQueryable<T>
7. Explain PLIQ?
    - Parallel linq it is the parallel execution of the LINQ to the objects. It endorses parallel programming and is closely associated with a parallel task library. 
8.   First vs FirstOrDefault()? If there is not an element in the result .First() returns the exception. FirstOrDefault() will not throw it.
9. Api? IQueryable or IEnumerable interfaces hosted in System.Linq namespace.
10. Linq compiled queries?
    - When need to execute a query repetedly linq enables us to create the query and compile it. 
    ```c#
    static readonly Func<AdventureWorksEntities, Decimal> s_compiledQuery3MQ = 
    CompiledQuery.Compile<AdventureWorksEntities, Decimal>(ctx => ctx.Products.Average(product => product.ListPrice));

    static void CompiledQuery3_MQ()
    {

        using (AdventureWorksEntities context = new AdventureWorksEntities())
        {
            Decimal averageProductPrice = s_compiledQuery3MQ.Invoke(context);

            Console.WriteLine("The average of the product list prices is $: {0}", averageProductPrice);
        }
    }
    ```
11. Anonymous types?
    - Types defined at run-time created by the compiler. We don't need to define a name but we can define properties` names and allocate values dynamically:
    ```c#
    var v = new {FirstProp = "1st prop"}
    Console.WriteLine(V,FirstProp);
    ```
    - They are useful in LINQ to store intermediate results.
12. Why SELECT clause comes after FROM?
    - Queries must be defined first, the FROM clause specifies the range or condition for selecting records, therefore it should act before the SELECT.
13. SelectMany() and Select() in LINQ?
    - Projection operators. Select() operator for selecting a value from the collection while SelectMany() is for selecting the values from the group of a collection (the nested collection).
14. Quantifier operators:
    1. Any()
    2. All()
    3. Contains()
15. Which extension method do we require to execute the parallel query?
    - AsParallel
16. Expression Lambda vs statement lambda?
    - Expression lambda for the construction of the expression trees. We cannot use statement lambda for that purpose.
17. Skip() vs SkipWhile() Skip() takes the integer argument and skips the top n numbers.
    - SkipWhile() continues to skip the elements until the input condition is true. 
18. Let clause for storing the result of the subexpression and using it in the consequent clauses. 
19. Take(1) vs First(). Take(1) returns an enumerable that only has one value and will give you an empty enumerable if the initial enumerable is empty. First() will return the first OBJECT and will throw an exception if the initial enumerable is empty.
20.  Action? Common delegates associated with the base class library of .NET. We can save only the methods with input parameters and void return types in Action.
21. Predicate delegate? In the predicate we are enabled to save only the methods with the one input parameter and the bool return type. They are helpful in the scenarios where the filter is required.
22. What are the sequence and query operators in LINQ?
    - Sequence: collection class we have to query. The element is a single item in the collection class. 
    - Query operators comprenhend the sequence as the input, impelement it and return the latest result sequence.
23. Extension methods? 
    - Static functions of the static class. We can invoke these methods the same as syntax of the instance method. We can use them when we don't need to modify the class. 
24. Difference EF vs Linq to SQL? 
    - Linq to Entities (EF) is an ORM that allows a wide definition of the object domain models and relashionships to varios ADO.NET data providers. 
    - Linq to SQL supporst only 1 to 1 mapping of the database views, tables, functions and SPs in SQL server. It is great for rapid data access production. 
25. Get a single row? Single() method. 
    I'd illustrate why you should NOT use one, when you mean the other.

    Note: In my code, I will typically use FirstOrDefault() and SingleOrDefault() but that's a different question.

    Take, for example, a table that stores Customers in different languages using a Composite Key ( ID, Lang ):
    ```c#
    DBContext db = new DBContext();
    Customer customer = db.Customers.Where( c=> c.ID == 5 ).First();
    ```
    This code above introduces a possible logic error ( difficult to trace ). It will return more than one record ( assuming you have the customer record in multiple languages ) but it will always return only the first one... which may work sometimes... but not others. It's unpredictable.

    Since your intent is to return a Single Customer use Single();

    The following would throw an exception ( which is what you want in this case ):
    ```c#
    DBContext db = new DBContext();
    Customer customer = db.Customers.Where( c=> c.ID == 5 ).Single();
        ```
    Then, you simply hit yourself on the forehead and say to yourself... OOPS! I forgot the language field! Following is the correct version:
    ```c#
    DBContext db = new DBContext();
    Customer customer = db.Customers.Where( c=> c.ID == 5 && c.Lang == "en" ).Single();
    ```
    First() is useful in the following scenario:
    ```c#
    DBContext db = new DBContext();
    NewsItem newsitem = db.NewsItems.OrderByDescending( n => n.AddedDate ).First();
    ```
    It will return ONE object, and since you're using sorting, it will be the most recent record that is returned.

    Using Single() when you feel it should explicitly always return 1 record will help you avoid logic errors.

26. FirstOrDefault() vs SingleOrDefault()?
    - If your result set returns 0 records:
        - SingleOrDefault returns the default value for the type (e.g. default for int is 0)
        - FirstOrDefault returns the default value for the type
    - If you result set returns 1 record:
        - SingleOrDefault returns that record
        - FirstOrDefault returns that record
    - **If your result set returns many records:**
        - SingleOrDefault throws an exception
        - FirstOrDefault returns the first record


## Practice

https://dotnetfiddle.net/qqYNum
```c#
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class Product{
	public int Id {get; set;}
	public string Name {get; set;}
	public double Price {get; set;}
	public string Color{get; set;}
	public int NameLength{get;set;}
}	
public static class ProductHelper{
	public static IEnumerable<Product> ByColor(this IEnumerable<Product> query, string color){
		return query.Where(prod => prod.Color == color);
	}
}
public class Program
{
	public static void Main()
	{
		// Data source
		var products = new List<Product>{
			new Product{Id = 1, Name = "Soup", Price = 2000, Color="Yellow"},
			new Product{Id = 2, Name = "Rice", Price = 2300, Color="White"},
			new Product{Id = 3, Name = "Water", Price = 850, Color = "Blue"},
			new Product{Id = 4, Name = "Soap", Price = 1000, Color="White"}
		};
		
		bool useQuerySyntax = false;
        IEnumerable<Product> result = new List<Product>();
		// LINQ Query 
		if(useQuerySyntax){
			//Using TakeWhile to select a certain amount of elements while condition is true
			result = (from prod in products
					  orderby prod.Price descending //need to order by price first
					  select prod).TakeWhile(p => p.Price >= 1000);
			//Get a specific amount of items using Take()
			/*result = (from prod in products
					 select prod).Take(2);
			//Assign values to properties using foreach (set operator set property value to an entire collection)
			result = (from prod in products
					  let tmp = prod.NameLength = prod.Name.Length
					  select prod).ToList();
			//Select a single item
			var singleItem = (from prod in products
							  select prod).Single(p => p.Color == "White"); 
			//Run-time exception (line 34): Sequence contains more than one matching element
			
			Console.WriteLine(singleItem.Name);
			//Select last item
			var lastItem = (from prod in products
							select prod).Last(p => p.Color == "White");
			
			Console.WriteLine(lastItem.Name);
			//Select first item
			var singleItem = (from prod in products
					  select prod).FirstOrDefault(p => p.Color == "White");
			
			Console.WriteLine(singleItem.Name);
			//Using extension methods
			result = (from prod in products
					select prod).ByColor("White");
			//Using where expression with two fields:
			result = from prod in products
				where prod.Name.Contains("a") && prod.Price >= 1000
				select prod;
			//Filtering data using where expression
			result = from prod in products
					where prod.Name.Contains("a")
					select prod;
			//Ordering using multiple fields:
			result = from prod in products
					orderby prod.Name descending, prod.Price
					select prod;
			//Ordering data in descending order
			result = from prod in products
					orderby prod.Name descending
					select prod;
			//Ordering data
			result = from prod in products
						orderby prod.Name
						select prod;
			//anonymous objects
			var anonymousProducts = (from prod in products
									select new{
										Identifier = prod.Id,
										NewName = prod.Name
									}).ToList();
			
			foreach(var p in anonymousProducts)
				Console.WriteLine("Name: {0} Identifier: {1}", p.NewName, p.Identifier );
				
			//Get specific columns
			result = (from prod in products
					  select new Product{
						  Id = prod.Id * 2,
						  Name = prod.Name
					  }).ToList();
					  
			//Using query syntax example:
			result =  from p in products
				select p;*/
		}else{
			//Using TakeWhile to select a certain amount of elements while condition is true
			result = products.Select(p => p).OrderByDescending(p => p.Price).TakeWhile(p=>p.Price >= 1000);
			//Get a specific amount of items using Take()
			/*result = products.Select(p=>p).Take(2);
			//Assign values to properties using foreach (set operator set property value to an entire collection)
			//ForEach in method syntax is void (does not return a collection)
			products.ForEach(p => p.NameLength = p.Name.Length); 
			foreach (var p in products)
            	Console.WriteLine("Name: {0} Id: {1} NameLength: {2}", p.Name, p.Id, p.NameLength);
			//Select a single item
			var singleItem = products.Select(p => p).Single(p => p.Color == "White");
			//Run-time exception (line 93): Sequence contains more than one matching element
			Console.WriteLine(singleItem.Name);
			//Select lastItem
			var lastItem = products.Select(p=>p).Last(p => p.Color == "White");
			Console.WriteLine(lastItem.Name);
			//Select a single item
			var singleItem = products.Select(p => p).FirstOrDefault();
			Console.WriteLine(singleItem.Name);
			//Using extension methods
			result = products.Select(p => p).ByColor("Yellow");
			//Using where expression with two fields:
			result = products.Select(p=>p).Where(p => p.Name.Contains("a") && p.Price >= 1000);
			//Filtering data using where expression
			result = products.Select(p => p).Where(p => p.Name.Contains("a"));
			//Ordering using multiple fields:
			result = products.Select(p => p).OrderByDescending(p => p.Name).ThenBy(p=>p.Price).ToList();
			//order by descending
			result = products.Select(p => p).OrderByDescending(p=> p.Name);
			//ordering data		
			result = products.Select(p => p).OrderBy(p => p.Name);
			//anonymous objects
			var anonymousProducts = products.Select(prod => new{
				Identifier = prod.Id,
				NewName = prod.Name
			}).ToList();
			
			foreach(var p in anonymousProducts)
				Console.WriteLine("Name: {0} Identifier: {1}", p.NewName, p.Identifier );
			
			//Get specific columns
			result = products.Select(prod => new Product{
				Id = prod.Id * 3,
				Name = prod.Name
			}).ToList();
			
			//Using method syntax
			result = products.Select(p => p);*/
		}
       
		// Query execution
        foreach (var p in result)
            Console.WriteLine("Name: {0} Id: {1} NameLength: {2} Price: {3}", p.Name, p.Id, p.NameLength, p.Price);
	}
}
```