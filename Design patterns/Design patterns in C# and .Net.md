# Design patterns:

- Are common architectural approaches.
- Creational patterns: builder, factories, prototype and singleton.
- Structural patterns: adapter, bridge, composite, decorator, facade, flyweight and proxy.
- Behavioral patterns: chain of responsability, command, interpreter, iterator, mediator, memento, null object, observer, state, strategy, template method, visitor.

## SOLID design principle:

- Single responsability principle:
    - Move the persistance methods to a separate class outside Journal because it had a lot of responsabilities.
    - Every class should have one responsability and one reason to change.
    ```c#
    namespace DesignPatterns
    {
        public class Journal
        {
            private readonly List<string> entries = new List<string>();
            private static int count = 0;
            public int AddEntry(string text)
            {
                entries.Add($"{++count}: {text}");
                return count;
            }

            public void RemoveEntry(int index)
            {
                entries.RemoveAt(index);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, entries);
            }
        }

        public class Persistence
        {
            public void SaveToFile(Journal j, string filename, bool overwrite = false)
            {
                if(overwrite || !File.Exists(filename))
                    File.WriteAllText(filename, j.ToString());
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
                var j = new Journal();
                j.AddEntry("I cried today");
                j.AddEntry("I ate a bug");
                Console.WriteLine(j);

                var p = new Persistence();
                var filename = @"journal.txt";
                p.SaveToFile(j, filename, true);
                Process.Start(filename);            }
        }
    }
    ```
- Open-Closed principle
    - Classes should be open for extension and close for modification. 
    - YOu can use inheritance (enhancing the capabilities using interfaces). 
    ```c#
    public enum Color
    {
        Red, Blue, Green
    }

    public enum Size
    {
        Small, Medium, Large
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if(name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }

            this.Name = name;
            this.Color = color;
            this.Size = size;
        }
    }

    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                    yield return p;
            }
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                    yield return p;
            }
        }

        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Color color, Size size)
        {
            foreach (var p in products)
            {
                if (p.Color == color && p.Size == size)
                    yield return p;
            }
        }
    }

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                    yield return item;
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };
            var pf = new ProductFilter();
            Console.WriteLine("Green product (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            Console.WriteLine("Green product (better):");

            var bf = new BetterFilter();
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            Console.WriteLine("Large blue items (better):");

            foreach (var p in bf.Filter(products, new AndSpecification<Product>(
                new ColorSpecification(Color.Blue),
                new SizeSpecification(Size.Large)
             )))
            {
                Console.WriteLine($" - {p.Name} is large and blue");
            }

        }
    }
    ```