# Design patterns:

- Are common architectural approaches.
- Creational patterns: builder, factories, prototype and singleton.
- Structural patterns: adapter, bridge, composite, decorator, facade, flyweight and proxy.
- Behavioral patterns: chain of responsability, command, interpreter, iterator, mediator, memento, null object, observer, state, strategy, template method, visitor.

## SOLID design principle:

- Single responsability principle:
    - Move the persistance methods to a separate class outside Journal because it had a lot of responsabilities.
    - Every class should have one responsability and one reason to change.
    - Separation of concerns. 
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
    - You can use inheritance (enhancing the capabilities using interfaces). 
    - You should able to extend the functionality but they should be close for modification. You shouldn't have to go back into BetterFilter and starting adding things. Instead if you want more functionality you make new classes and you implement ISpecification and you feed those into something that has already been shipped. You don't to ship the functionality of BetterFilter to your customers but you can ship additional modules which implement a high specification and which make use of better filter. 
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
- Liskov substitution principle:
    - You should be able to upcast to your base type and the operation should be still generally OK. The Square should still behave as a Square even when you're getting a reference to a rectangle for it. 
    - This breaks the Liskov substitution principle
    ```c#
    public class Rectangule
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangule(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Rectangule()
        {

        }

        public override string ToString()
        {
            return $"Width: {this.Width} Height: {this.Height}";
        }
    }

    public class Square : Rectangule
    {
        public new int Width {
            set{ base.Width = base.Height = value; }
        }

        public new int Height
        {
            set { base.Height = base.Width = value; }
        }
    }

    public class Program
    {
        static int Area(Rectangule rect) => rect.Width * rect.Height;

        static void Main(string[] args)
        {
            Rectangule rect = new Rectangule(5, 6);
            Console.WriteLine($"{rect} has area of: {Area(rect)}");

            Square sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area of: {Area(sq)}");

            Rectangule sq2 = new Square();
            sq2.Width = 4;
            Console.WriteLine($"{sq2} has area of: {Area(sq2)}");
        }
    }
    /*   
    Width: 5 Height: 6 has area of: 30
    Width: 4 Height: 4 has area of: 16
    Width: 4 Height: 0 has area of: 0*/
    ```
- To fix it: 
    ```c#
    public class Rectangule
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangule(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Rectangule()
        {

        }

        public override string ToString()
        {
            return $"Width: {this.Width} Height: {this.Height}";
        }
    }

    public class Square : Rectangule
    {
        public override int Width {
            set{ base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Height = base.Width = value; }
        }
    }
    public class Program
    {
        static int Area(Rectangule rect) => rect.Width * rect.Height;

        static void Main(string[] args)
        {
            Rectangule rect = new Rectangule(5, 6);
            Console.WriteLine($"{rect} has area of: {Area(rect)}");

            Square sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area of: {Area(sq)}");

            Rectangule sq2 = new Square();
            sq2.Width = 4;
            Console.WriteLine($"{sq2} has area of: {Area(sq2)}");
        }
    }

    /*Width: 5 Height: 6 has area of: 30
    Width: 4 Height: 4 has area of: 16
    Width: 4 Height: 4 has area of: 16*/
    ```
- Interface segregation principle
    - Your interfaces should be segragated so that nobody who implements them has to implement functions that they don't actually need. 
    - YAGNI (you ain't going to need it). 
    ```c#
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMultifunctionDevice : IPrinter, IScanner
    {

    }

    public class MultifunctionMachine : IMultifunctionDevice
    {
        private IPrinter printer;
        private IScanner scanner;

        public MultifunctionMachine(IPrinter printer, IScanner scanner)
        {
            this.printer = printer;
            this.scanner = scanner;
        }

        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }
    }

    public class Document
    {

    }
    ```
- The dependency inversion principle:
    - The high level parts of the system should not depend on low level parts of the system directly. 
    - They should depend on some kind of abstraction. 
    - The following example violates the dependency inversion principle because it exposes directly a property from the datastore to the high level research class and it cannot change (the tuples for a dictionary for example) without breaking research:
    ```c#
      public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
    }

    //low-level (data store)
    public class Relationships
    {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public List<(Person, Relationship, Person)> Relations => relations;
    }

    //high level
    public class Research
    {
        public Research(Relationships relationships)
        {
            var relations = relationships.Relations;

            foreach (var r in relations.Where(
                x => x.Item1.Name == "John"
                && x.Item2 == Relationship.Parent
            ))
            {
                Console.WriteLine($"John has a child called: {r.Item3.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
    ```
    - To fix it we can define an interface and implement it in the low level and also pass it as an argument in the high level instead of passing the class implementation directly. Now relationships can change the way it stores the information without breaking research because it is never exposed to the direct level module directly. 
    ```c#
     public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    //low-level
    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations.Where(
             x => x.Item1.Name == name
             && x.Item2 == Relationship.Parent
            ).Select(r => r.Item3);
        }
    }

    //high level
    public class Research 
    {
        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("John"))
            {
                Console.WriteLine($"John has a child called: {p.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
    ```