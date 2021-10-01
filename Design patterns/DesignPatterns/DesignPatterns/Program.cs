using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DesignPatterns
{
    #region SOLID principles
    #region Single Responsability
    /*public class Journal
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
            if (overwrite || !File.Exists(filename))
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
            Process.Start(filename);
        }
    }*/
    #endregion
    #region Open Close
    /*public enum Color
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
    }*/
    #endregion
    #region Liskov Substitution Principle
    /*public class Rectangule
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
    }*/
    #endregion
    #region Interface Segregation Principle

    /*public interface IPrinter
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
    public class Program
    {
        static void Main(string[] args)
        {

        }
    }*/
    #endregion
    #region Dependency inversion principle
    /*public enum Relationship
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

        //public List<(Person, Relationship, Person)> Relations => relations;
    }

    //high level
    public class Research 
    {
        //public Research(Relationships relationships)
        //{
          //  var relations = relationships.Relations;

            //foreach (var r in relations.Where(
              //  x => x.Item1.Name == "John"
                //&& x.Item2 == Relationship.Parent
           // ))
            //{
              //  Console.WriteLine($"John has a child called: {r.Item3.Name}");
           // }
        //}

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
    }*/

    #endregion
    #endregion
    #region Builder
    #region Classic builder
    /*public class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            this.Name = name;
            this.Text = text;
        }

        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.AppendLine($"{i}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var htmlElement in Elements)
            {
                sb.Append(htmlElement.ToStringImpl(indent + 1));
            }
            sb.AppendLine($"{i}</{Name}>");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder
    {
        HtmlElement root = new HtmlElement();
        private readonly string rootName;

        public HtmlBuilder(string rootName)
        {
            root.Name = rootName;
            this.rootName = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement()
            {
                Name = rootName
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HtmlBuilder("ul");
            builder.AddChild("li", "hello").AddChild("li", "world");
            Console.WriteLine(builder.ToString());
        }
    }*/
    #endregion
    #region Recursive building
    /*public class Person
    {
        public string Name;
        public string Position;

        public class Builder : PersonJobBuilder<Builder>
        {

        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)} : {Name}, {nameof(Position)}: {Position}";
        }
    }

    public abstract class PersonBuilder
    {
        protected Person person = new Person();

        public Person Build() { return person; }
    }

    public class PersonInfoBuilder<T> : PersonBuilder
        where T : PersonInfoBuilder<T>
    {
        public T Called(string name)
        {
            person.Name = name;
            return (T)this;
        }
    }

    public class PersonJobBuilder<T> : PersonInfoBuilder<PersonJobBuilder<T>>
        where T : PersonJobBuilder<T>
    {
        public T WorksAAsA(string position)
        {
            person.Position = position;
            return (T)this;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //var builder = new PersonJobBuilder();
            //builder.Called("diego").WorksAs("dev");
            var person = Person.New.Called("diego").WorksAAsA("dev").Build();
            Console.WriteLine(person.ToString());
        }
    }*/
    #endregion
    #region Stepwise Builder
    /*public enum CarType
    {
        Sedan,
        Crossover
    }

    public class Car
    {
        public CarType Type;
        public int WheelSize;
    }

    public interface ISpecifyCarType
    {
        ISpecifyWheelSize OfType(CarType type);
    }

    public interface ISpecifyWheelSize
    {
        IBuildCar WithWheels(int wheelSize);
    }

    public interface IBuildCar
    {
        public Car Build();
    }

    public class CarBuilder
    {
        private class Impl : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
        {
            private Car car = new Car();

            public Car Build()
            {
                return car;
            }

            public ISpecifyWheelSize OfType(CarType type)
            {
                car.Type = type;
                return this;
            }

            public IBuildCar WithWheels(int wheelSize)
            {
                switch (car.Type)
                {
                    case CarType.Crossover when wheelSize < 10 || wheelSize > 20:
                    case CarType.Sedan when wheelSize < 15 || wheelSize > 17:
                        throw new ArgumentNullException($"Wrong size of wheel for {car.Type}");
                }
                car.WheelSize = wheelSize;
                return this;
            }
        }

        public static ISpecifyCarType Create()
        {
            return new Impl();
        }
    }

    class Program
    {
        static void Main(string[] args) { 
            var car = CarBuilder.Create()// ISpecifyCarType
                .OfType(CarType.Crossover)//ISpecifyWheelSize
                .WithWheels(18) //IBuildCar
                .Build();
            Console.WriteLine($"{nameof(car.Type)} : {car.Type} {nameof(car.WheelSize)} : {car.WheelSize}");

            var create = CarBuilder.Create();
        }
    }*/
    #endregion
    #region Functional builder
    public class Person
    {
        public string Name, Position;
    }

    public sealed class PersonBuilder
    {
        private readonly List<Func<Person, Person>> actions = new List<Func<Person, Person>>();

        public PersonBuilder Called(string name) => Do(p => p.Name = name);
        public PersonBuilder Do(Action<Person> action) => AddAction(action);

        private PersonBuilder AddAction(Action<Person> action)
        {
            actions.Add(p =>
            {
                action(p);
                return p;
            });
            return this;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
        }
    }
    #endregion
    #endregion
}
