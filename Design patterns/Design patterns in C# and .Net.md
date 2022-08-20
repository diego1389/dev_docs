


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
    - You should be able to extend the functionality but they should be close for modification. You shouldn't have to go back into BetterFilter and starting adding things. Instead if you want more functionality you make new classes and you implement ISpecification and you feed those into something that has already been shipped. You don't to ship the functionality of BetterFilter to your customers but you can ship additional modules which implement a high specification and which make use of better filter. 
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
## Builder

- Gamma categorization:
    1. Creational patterns: deal with the creation (construction) of objects.
        - Explicit (constructor) vs implicit (DI, reflection, etc).
        - Wholesale (single statement) vs piecewise (step-by-step).
    2. Structural patterns: structure of the classes (class members).
        - Many patterns are wrappers that mimic the underlying class interface.
        - Good API design.
    3. Behavioral patterns:
        - They are all different (no central theme).
        - They solve a particular problem in a particular way.
- Builder: some objects are simple and can be created in a single constructor call, 
    - Other objects require a lot of ceremony to create.
    - Having an object with 10 constructor arguments is not productive.
    - Instead, opt for piecewise constructor.
    - When a piecewise object construction is complicated provide an API for doing it succintly.
    ```c#
    public class HtmlElement
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
    }
    ```
- The problem: when you call the "Called" method it returns a PersonInfoBuilder and it doesn't have a definition for WorksAsA.
    ```c#
    public class Person
    {
        public string Name;
        public string Position;
    }

    public class PersonInfoBuilder
    {
        protected Person person = new Person();
        public PersonInfoBuilder Called(string name)
        {
            person.Name = name;
            return this;
        }
    }

    public class PersonJobBuilder : PersonInfoBuilder
    {
        public PersonJobBuilder WorksAAsA(string position)
        {
            person.Position = position;
            return this;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new PersonJobBuilder();
            builder.Called("diego").WorksAsA("dev"); /*Error PersonInfoBuilder does not contain a definition for WorkAsA*/
        }
    }
    ```
- The solution: fluent builder with recursive generics:
    - You need a return type to refer not to the current object but the kind of this that you get throught inheritance.
    ```c#
        public class Person
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

    /* we're only allowing the situation where T actually refers to the object inheriting form this object: Foo : Bar<Foo>*/
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
            /*var builder = new PersonJobBuilder();
            builder.Called("diego").WorksAs("dev");*/
            var person = Person.New.Called("diego").WorksAAsA("dev").Build();
            Console.WriteLine(person.ToString());
        }
    }
    ```
- Stepwise builder:
    - A chain that is enforce through various interfaces. 
    - When you need the creation to be in certain order.
    ```c#
    public enum CarType
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
        }
    }
    ```
## State

- A pattern in which the object's behaviour is determined by its state. An object transitions from one state to another. 
- A formalized construct which manages state and transitions is called a state machine. 
- **Handmade state machines:**

- Add a State and Trigger enums to list the possible states and the triggers that fire those transitions:
    ```c#
      public enum State
    {
        OffHook,
        Connecting,
        Connedted,
        OnHold
    }

    public enum Trigger
    {
        CallDialed,
        HangUp,
        CallConnected,
        PlacedOnHold,
        TakeOffHold,
        LeftMessage
    }
    ```
- Create the state machine:
    ```c#
    class Program
    {
        private static Dictionary<State, List<(Trigger, State)>> rules
            = new Dictionary<State, List<(Trigger, State)>>
            {
                [State.OffHook] = new List<(Trigger, State)>
                {
                    (Trigger.CallDialed, State.Connecting)
                },
                [State.Connecting] = new List<(Trigger, State)>
                {
                    (Trigger.HangUp, State.OffHook),
                    (Trigger.CallConnected, State.Connedted)
                },
                [State.Connedted] = new List<(Trigger, State)>
                {
                    (Trigger.LeftMessage,State.OffHook),
                    (Trigger.HangUp, State.OffHook),
                    (Trigger.PlacedOnHold, State.OnHold)
                },
                [State.OnHold] = new List<(Trigger, State)>
                {
                    (Trigger.TakeOffHold, State.Connedted),
                    (Trigger.HangUp, State.OffHook)
                }
            };

        static void Main(string[] args)
        {
            //initial state
            var state = State.OffHook;
            while (true)
            {
                Console.WriteLine($"The phone is currently {state}");
                Console.WriteLine($"Select a trigger:");
                //display the possible triggers for that state
                for (int i = 0; i < rules[state].Count; i++)
                {
                    var (t,_) = rules[state][i];
                    Console.WriteLine($"{i}. {t}");
                }

                int input = int.Parse(Console.ReadLine());
                //change the state according with the trigger selected by the user
                var (_, s) = rules[state][input];
                state = s;
            }
        }
    }
    ```
- Switch-based state machine
    - It is useful when you have small set of transitions.
    - Add an enum with the possible states:
    ```c#
     public enum State
    {
        Locked,
        Failed,
        Unlocked
    }
    ```
    - Add the switch-based state machine:
    ```c#
    class Program
    {
        static void Main(string[] args)
        {
            string code = "1234";
            var state = State.Locked;

            var entry = new StringBuilder();

            while (true)
            {
                switch (state)
                {
                    case State.Locked:
                        entry.Append(Console.ReadKey().KeyChar);
                        if (entry.ToString() == code)
                        {
                            state = State.Unlocked;
                            break;
                        }
                        if (!code.StartsWith(entry.ToString()))
                        {
                            //you can also use goto to go to another case without changing the state
                            state = State.Failed;
                            break;
                        }
                        break;
                    case State.Failed:
                        Console.CursorLeft = 0;
                        Console.WriteLine("FAILED");
                        entry.Clear();
                        state = State.Locked;
                        break;
                    case State.Unlocked:
                        Console.CursorLeft = 0;
                        Console.WriteLine("UNLOCKED");
                        return;
                }
            }
        }
    }
    ```
- Switch expressions
    - Not switch statement. 
    - Better approach than the previous one.
    - Add state and action enums:
    ```c#
    public enum Chest
    {
        Open,
        Closed,
        Locked
    }

    public enum ChestAction
    {
        Open,
        Close
    }
    ```
    - Define the informal state machine:
    ```c#
    class Program
    {
        static Chest Manipulate(Chest chest, ChestAction action, bool haveKey)
                => (chest, action, haveKey) switch
                {
                    (Chest.Locked, ChestAction.Open, true) => Chest.Open,
                    (Chest.Closed, ChestAction.Open, _) => Chest.Open,
                    (Chest.Open, ChestAction.Close, true) => Chest.Locked,
                    (Chest.Open, ChestAction.Close, false) => Chest.Closed,
                    _ => chest
                };

        static void Main(string[] args)
        {
            var chest = Chest.Locked;
            Console.WriteLine($"Chest is {chest}");
            chest = Manipulate(chest, ChestAction.Open, true);
            Console.WriteLine($"Chest is {chest}");
            chest = Manipulate(chest, ChestAction.Close, false);
            Console.WriteLine($"Chest is {chest}");
            chest = Manipulate(chest, ChestAction.Close, false);
            Console.WriteLine($"Chest is {chest}");
        }
    }
    ```
- State machine with stateless
    - Create a state machine using stateless library.
    - Add state and activity for transitions:
    ```c#
    public enum Health
    {
        NonReproductive,
        Pregnant,
        Reproductive
    }

    public enum Activity
    {
        GiveBirth,
        ReachPuberty,
        HaveAbortion,
        HaveUnprotectedSex,
        Historectomy
    }
    ```
    - Use stateless to configure the state machine:
    ```c#
     class Program
    {
        public static bool ParentsNotWatching { get; private set; }

        static void Main(string[] args)
        {
            var machine = new StateMachine<Health, Activity>(Health.NonReproductive);
            machine.Configure(Health.NonReproductive)
                .Permit(Activity.ReachPuberty, Health.Reproductive);
            machine.Configure(Health.Reproductive)
                .Permit(Activity.Historectomy, Health.NonReproductive)
                .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
                () => ParentsNotWatching);
            machine.Configure(Health.Pregnant)
             .Permit(Activity.GiveBirth, Health.Reproductive)
             .Permit(Activity.HaveAbortion, Health.Reproductive);

            
        }
    }
    ```
## Abstract factory

- This creates a set of related objects or dependent objects. 
- The "family" of objects created by the factory is determined at run-time depending on the selection of concrete factory classes.
- An abstract factory pattern acts as a super-factory that creates other factories. An abstract factory interface is responsible for creating a set of related objects or dependent objects without specifying their concrete classes. 
-  We can say it is just an object maker which can create more than one type of object.

- Elements: 
    1. **Client** This class uses the Abstract Factory and Abstract Product interfaces to create a family of related objects.
    2. **Abstract Factory** This is an interface that creates abstract products.
    3. **Abstract Product** This is an interface that declares a type of product.
    4. **Concrete Factory** This is a class that implements the abstract factory interface to create concrete products.
    5. **Concrete Product**  This is a class that implements the abstract product interface to create products.
- Real life example:
    - Client.cs
    ```c#
    public class Client
    {
        private Packaging _packaging;
        private DeliveryDocument _deliveryDocument;

        public Client(PcknDelvFactory factory)
        {
            _packaging = factory.CreatePackaging();
            _deliveryDocument = factory.CreateDeliveryDocument();
        }

        public Packaging ClientPackaging
        {
            get { return _packaging;  }
        }

        public DeliveryDocument ClientDocument
        {
            get { return _deliveryDocument; }
        }
    }
    ```
    - PcknDelvFactory.cs
    ```c#
    public abstract class PcknDelvFactory
    {
        public abstract Packaging CreatePackaging();
        public abstract DeliveryDocument CreateDeliveryDocument();
    }

    public abstract class Packaging { }
    public class StandardPackaging : Packaging { }
    public class ShockProofPackaging : Packaging { }
    public abstract class DeliveryDocument { }
    public class Postal : DeliveryDocument { }
    public class Courier : DeliveryDocument { }
    ```
    - StandardFactory.cs
    ```c#
    public class StandardFactory : PcknDelvFactory
    {
        public override DeliveryDocument CreateDeliveryDocument()
        {
            return new Postal();
        }

        public override Packaging CreatePackaging()
        {
            return new StandardPackaging();
        }
    }
    ```
    - DelicateFactory
    ```c#
    public class DelicateFactory : PcknDelvFactory
    {
        public override DeliveryDocument CreateDeliveryDocument()
        {
            return new Courier();
        }

        public override Packaging CreatePackaging()
        {
            return new ShockProofPackaging();
        }
    }
    ```
    - Program.cs
    ```c#
    static void Main(string[] args)
    {
        PcknDelvFactory sf = new StandardFactory();
        Client standard = new Client(sf);

        Console.WriteLine(standard.ClientPackaging.GetType().ToString());
        Console.WriteLine(standard.ClientDocument.GetType().ToString());
        Console.WriteLine("------------------");

        PcknDelvFactory df = new DelicateFactory();
        Client delicate = new Client(df);

        Console.WriteLine(delicate.ClientPackaging.GetType().ToString());
        Console.WriteLine(delicate.ClientDocument.GetType().ToString());
        /*
        DesignPatterns2.AbstractFactory.StandardPackaging
        DesignPatterns2.AbstractFactory.Postal
        ------------------
        DesignPatterns2.AbstractFactory.ShockProofPackaging
        DesignPatterns2.AbstractFactory.Courier             
        */
    }
    ```
- The example code above creates two client objects, each passing to a different type of factory constructor. Types of generated objects are accessed through the client's properties.  

## Factory method

- Defines an interface for creating an object, but let subclasses decide which class to instantiate. This pattern lets a class defer instantiation to subclasses.

- Elements:
    1. **Product:** defines the interface of objects the factory method creates.
    2. **Concrete product:** implements the Product interface.
    3. **Creator:** declares the factory method, which returns an object of type Product. Creator may also define a default implementation of the factory method that returns a default ConcreteProduct object.
    4. **ConcreteCreator:** overrides the factory method to return an instance of a ConcreteProduct.
- Real life example:
    - The Factory method offering flexibility in creating different documents. The derived Document classes Report and Resume instantiate extended versions of the Document class. Here, the Factory Method is called in the constructor of the Document base class.
    - Page.cs (The 'Product' abstract class)
    ```c#
     public abstract class Page
    {
        public abstract void DisplayText();
    }
    ```
    - Education page (concrete product classes)
    ```c#
     public class EducationPage : Page
    {
        public override void DisplayText()
        {
            Console.WriteLine("Education page info");
        }
    }
     public class ConclusionPage : Page
    {
        public override void DisplayText()
        {
            Console.WriteLine("Conclusion page info");
        }
    }
    public class IntroductionPage : Page
    {
        public override void DisplayText()
        {
            Console.WriteLine("Introduction page info");
        }
    }
    public class ResultsPage : Page
    {
        public override void DisplayText()
        {
            Console.WriteLine("Result page info");
        }
    }
     public class SkillsPage : Page
    {
        public override void DisplayText()
        {
            Console.WriteLine("Skills page info");
        }
    }
    ```
    - Document.cs (Creator)
    ```c#
    public abstract class Document
    {
        private List<Page> _pages = new List<Page>();
        // Constructor calls abstract Factory method
        public Document()
        {
            this.CreatePages();
        }
        public List<Page> Pages
        {
            get { return _pages; }
        }
        // Factory Method
        public abstract void CreatePages();
    }
    ```
    - Report.cs (Concrete creator)
    ```c#
     public class Report : Document
    {
        public override void CreatePages()
        {
            Pages.Add(new IntroductionPage());
            Pages.Add(new ResultsPage());
            Pages.Add(new ConclusionPage());
        }
    }
    ```
    - Resume.cs (Concrete creator)
    ```c#
    public class Resume : Document
    {
        public override void CreatePages()
        {
            Pages.Add(new SkillsPage());
            Pages.Add(new EducationPage());
        }
    }
    ```
    - Program.cs
    ```c#
    Document[] documents = new Document[2];
    documents[0] = new Report();
    documents[1] = new Resume();

    foreach (var document in documents)
    {
        Console.WriteLine(document.GetType().Name);
        foreach (var page in document.Pages)
        {
            page.DisplayText();
        }

    }
    //Report
    //Introduction page info
    //Result page info
    //Conclusion page info
    //Resume
    //Skills page info
    //Education page info
    ```
    - 
## Bridge pattern

- It makes a bridge between two components. Here the component may be two classes or any other entity. So the Bridge Design Pattern basically makes a channel between two components. And in this way it helps to create a de-couple architecture. We can communicate with two classes through the bridge component without changing existing class definitions.

- It makes abstraction over implementation.

- Real life example:
    - Your manager said that they want to use both C# and VB versions associated with sending mail from a database (so, in total three different ways). Now, let's implement this scenario using the Bridge Design Pattern. 
    - IMessage.cs
    ```c#
    public interface IMessage
    {
        void Send();
    }
    ```
    - CSharp_Mail.cs
    ```c#
    public class CSharp_Mail : IMessage
    {
        public void Send()
        {
            Console.WriteLine("Mail send from C# code");
        }
    }
    ```
    - VB_Mail.cs
    ```c#
    public class VB_Mail : IMessage
    {
        public void Send()
        {
            Console.WriteLine("Mail send from VB code");
        }
    }
    ```
    - The MailSendBridge class is creating one abstraction over the implementation of the actual mail sending mechanism that was defined by a different mail sending class
    ```c#
    public class MailSendBridge
    {
        public void SendFrom(IMessage mailProvider)
        {
            mailProvider.Send();
        }
    }
    ```
    - Program.cs
    ```c#
    class Program
    {
        static void Main(string[] args)
        {
            MailSendBridge mb = new MailSendBridge();
            CSharp_Mail csProvider = new CSharp_Mail();
            VB_Mail vbProvider = new VB_Mail();
            mb.SendFrom(csProvider);
            mb.SendFrom(vbProvider);
        }
    }
    ```
## Prototype

- Prototype means making a copy of something that exists.
- Real life example: 
    - Consider that you want to create one copy of an object multiple times. For your birthday you want to send an invitation letter to your friends, now the content and sender name will remain the same whereas the recipient name will only change. In this situation we can use a prototype of the invitation card for multiple friends.
    - InvitationCard.cs
    ```c#
    public class InvitationCard
    {
        public String To;
        public String Title;
        public String Content;
        public String SendBy;
        public DateTime Date;

        public String p_To
        {
            get { return To; }
            set { To = value; }
        }
        public String p_Title
        {
            get { return Title; }
            set { Title = value; }
        }
        public String p_content
        {
            get { return Content; }
            set { Content = value; }
        }
        public String p_SendBy
        {
            get { return SendBy; }
            set { SendBy = value; }
        }
        public DateTime p_Date
        {
            get { return Date; }
            set { Date = value; }
        }

        public InvitationCard CloneMe()
        {
            return (InvitationCard)this.MemberwiseClone();
        }
    }
    ```
    - Program.cs
    ```c#
    class Program
    {
        static void Main(string[] args)
        {
            InvitationCard obj1 = new InvitationCard();
            obj1.p_To = "Ram";
            obj1.p_Title = "My birthday invitation";
            obj1.p_content = "Hey guys !! I am throwing a cheers party in my home";
            obj1.SendBy = "Sourav";
            obj1.p_Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            //Here our first object has created  
            InvitationCard[] objList = new InvitationCard[5];
            String[] nameList = { "Ram", "Shyam", "Hari", "Tapan", "Sukant" };
            int i = 0;
            foreach (String name in nameList)
            {
                //objList[i] = new InvitationCard();  
                objList[i] = obj1.CloneMe();
                objList[i].p_To = nameList[i];
                i++;
            }
            // Print all Invitation Card here  
            foreach (InvitationCard obj in objList)
            {
                Console.WriteLine("To :- " + obj.p_To);
                Console.WriteLine("Title :- " + obj.p_Title);
                Console.WriteLine("Content :- " + obj.p_content);
                Console.WriteLine("Send By :- " + obj.p_SendBy);
                Console.WriteLine("Date :- " + obj.Date);
                Console.WriteLine("\n");
            }
        }
    }
    ```
## Decorator

- Attaches additional responsibilities to an object dynamically. This pattern provide a flexible alternative to subclassing for extending functionality.

- Elements 
    1. **Component:** defines the interface for objects that can have responsabilities added to them. 
    2. **Concrete component:** defines an object to which additional responsabilities can be attached.
    3. **Decorator:** mantains a reference to a Component object and defines an interface that conforms to components interface.
    4. **Concrete decorator:** adds responsabilities to the component.

- Real life example
    - LibraryItem.cs (The 'Component' abstract class)
    ```c#
    public abstract class LibraryItem
    {
        public int NumCopies { get; set; }
        public abstract void Display();
    }
    ```
    - Book.cs (A 'Concrete component' abstact)
    ```c#
    public class Book : LibraryItem
    {
        private string author;
        private string title;
        // Constructor
        public Book(string author, string title, int numCopies)
        {
            this.author = author;
            this.title = title;
            this.NumCopies = numCopies;
        }
        public override void Display()
        {
            Console.WriteLine("\nBook ------ ");
            Console.WriteLine(" Author: {0}", author);
            Console.WriteLine(" Title: {0}", title);
            Console.WriteLine(" # Copies: {0}", NumCopies);
        }
    }
    ```
    - Video.cs (A 'Concrete component' abstact)
    ```c#
    public class Video : LibraryItem
    {
        private string director;
        private string title;
        private int playTime;
        // Constructor
        public Video(string director, string title, int numCopies, int playTime)
        {
            this.director = director;
            this.title = title;
            this.NumCopies = numCopies;
            this.playTime = playTime;
        }
        public override void Display()
        {
            Console.WriteLine("\nVideo ----- ");
            Console.WriteLine(" Director: {0}", director);
            Console.WriteLine(" Title: {0}", title);
            Console.WriteLine(" # Copies: {0}", NumCopies);
            Console.WriteLine(" Playtime: {0}\n", playTime);
        }
    }
    ```
    - Decorator.cs (Decorator abstract class)
    ```c#
     public abstract class Decorator : LibraryItem
    {
        protected LibraryItem libraryItem;
        // Constructor
        public Decorator(LibraryItem libraryItem)
        {
            this.libraryItem = libraryItem;
        }
        public override void Display()
        {
            libraryItem.Display();
        }
    }
    ```
    - Borrowable.cs (Concrete decorator)
    ```c#
    public class Borrowable : Decorator
    {
        protected readonly List<string> borrowers = new List<string>();
        // Constructor
        public Borrowable(LibraryItem libraryItem)
            : base(libraryItem)
        {
        }
        public void BorrowItem(string name)
        {
            borrowers.Add(name);
            libraryItem.NumCopies--;
        }
        public void ReturnItem(string name)
        {
            borrowers.Remove(name);
            libraryItem.NumCopies++;
        }
        public override void Display()
        {
            base.Display();
            foreach (string borrower in borrowers)
            {
                Console.WriteLine(" borrower: " + borrower);
            }
        }
    }
    ```
    - Program.cs
    ```c#
    class Program
    {
        static void Main(string[] args)
        {
            // Create book
            Book book = new Book("Worley", "Inside ASP.NET", 10);
            book.Display();
            // Create video
            Video video = new Video("Spielberg", "Jaws", 23, 92);
            video.Display();
            // Make video borrowable, then borrow and display
            Console.WriteLine("\nMaking video borrowable:");
            Borrowable borrowvideo = new Borrowable(video);
            borrowvideo.BorrowItem("Customer #1");
            borrowvideo.BorrowItem("Customer #2");
            borrowvideo.Display();
        }

            /*
            Book ----- 
             Author: Worley
             Title: Inside ASP.NET
             # Copies: 10

            Video ----- 
             Director: Spielberg
             Title: Jaws
             # Copies: 23
             Playtime: 92


            Making video borrowable:

            Video ----- 
             Director: Spielberg
             Title: Jaws
             # Copies: 21
             Playtime: 92

             borrower: Customer #1
             borrower: Customer #2
            */
    }
    ```
## Facade

- The Facade design pattern provides a unified interface to a set of interfaces in a subsystem. This pattern defines a higher-level interface that makes the subsystem easier to use.
- Elements
    1. **Facade** 
    - Knows which subsystem classes are responsible for a request.
    - Delegates client requests to appropriate subsystem objects.
    2. **Subsystem classes**
    - Implement subsystem functionality.
    - Handle work assigned by the Facade object.
    - Have no knowledge of the facade and keep no reference to it.
- Real life example
    - A MortgageApplication object which provides a simplified interface to a large subsystem of classes measuring the creditworthiness of an applicant.
    - Bank.cs (subsystem class)
    ```c#
    public class Bank
    {
        public bool HasSufficientSavings(Customer c, int amount)
        {
            Console.WriteLine("Check bank for " + c.Name);
            return true;
        }
    }
    ```
    - Credit.cs (subsystem class)
    ```c#
    public class Credit
    {
        public bool HasGoodCredit(Customer c)
        {
            Console.WriteLine("Check credit for " + c.Name);
            return true;
        }
    }
    ```
    - Customer.cs
    ```c#
    public class Customer
    {
        private string name;
        // Constructor
        public Customer(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get { return name; }
        }
    }
    ```
    - Loan.cs (subsystem class)
    ```c#
    public class Loan
    {
        public bool HasNoBadLoans(Customer c)
        {
            Console.WriteLine("Check loans for " + c.Name);
            return true;
        }
    }
    ```
    - Mortgage.cs (Facade class)
    ```c#
    public class Mortgage
    {
        Bank bank = new Bank();
        Loan loan = new Loan();
        Credit credit = new Credit();
        public bool IsEligible(Customer cust, int amount)
        {
            Console.WriteLine("{0} applies for {1:C} loan\n",
                cust.Name, amount);
            bool eligible = true;
            // Check creditworthyness of applicant
            if (!bank.HasSufficientSavings(cust, amount))
            {
                eligible = false;
            }
            else if (!loan.HasNoBadLoans(cust))
            {
                eligible = false;
            }
            else if (!credit.HasGoodCredit(cust))
            {
                eligible = false;
            }
            return eligible;
        }
    }
    ```
    - Program.cs
    ```c#
     class Program
    {
        static void Main(string[] args)
        {
            var mortgage = new Mortgage();
            var customer = new Customer("Diego");
            bool isEligible = mortgage.IsEligible(customer, 170000);

            Console.WriteLine($@"{customer.Name} has been  {(isEligible ? "Approved" : "Rejected")}");

        }
    }

    //Check bank for Diego
    //Check loans for Diego
    //Check credit for Diego
    //Diego has been  Approved
    ```
## FLyweight

- The Flyweight design pattern uses sharing to support large numbers of fine-grained objects efficiently.
- Elements
    1. **Flyweight:** declares an interface through which flyweights can receive and act on extrinsic state (unique for each entity).
    2. **Concrete Flyweight:** implements the Flyweight interface and adds storage for intrinsic state (sharable). Any state it stores must be intrinsic (independent of the concrete flyweight object's context).
    3. **UnsharedConcreteFlyweight:** Not all Flyweight subclasses must be shared. The flyweight interface enables sharing but it does not enforce it. 
    4. **Flyweight factory:** Creates and manages flyweight objects. Ensures that flyweight are shared properly. When a client requests a flyweight, the Flyweight factory objects assets an existing intance or creates one. 
    5. **Client:** mantains a reference to flyweights. Computes or stores the extrinsic state of flyweights. 
- Real life example: 
    - A video game that uses flyweight pattern to get new characters.
    - Character.cs (Flyweight abstract class)
    ```c#
    public abstract class Character
    {
        public int HealthPoints { get; set; }
        public int Damage { get; set; }
        public abstract void Display(int damage);
    }
    ```
    - Wizard.cs (ConcreteFlyweight class)
    ```c#
     public class Wizard : Character
    {
        // Constructor
        public Wizard()
        {
            HealthPoints = 50;
        }
        public override void Display(int damage)
        {
            this.Damage = damage;
            string label = $"The {this.GetType().Name} has {HealthPoints} health points and {this.Damage} of damage";
            Console.WriteLine(label);
        }
    }
    ```
    - Warrior.cs (ConcreteFlyweight class)
    ```c#
    public class Warrior : Character
    {
        // Constructor
        public Warrior()
        {
            HealthPoints = 70;
        }
        public override void Display(int damage)
        {
            this.Damage = damage;
            string label = $"The {this.GetType().Name} has {HealthPoints} health points and {this.Damage} of damage";
            Console.WriteLine(label);
        }
    }
    ```
    - Healer.cs (ConcreteFlyweight class)
    ```c#
    public class Healer : Character
    {
        // Constructor
        public Healer()
        {
            HealthPoints = 40;
        }
        public override void Display(int damage)
        {
            this.Damage = damage;
            string label = $"The {this.GetType().Name} has {HealthPoints} health points and {this.Damage} of damage";
            Console.WriteLine(label);
        }
    }
    ```
    - CharacterFactory.cs (Flyweight factory)
    ```c#
    public class CharacterFactory
    {
        private Dictionary<CharacterTypes, Character> characters = new Dictionary<CharacterTypes, Character>();
        public Character GetCharacter(CharacterTypes key)
        {
            // Uses "lazy initialization"
            Character character = null;
            if (characters.ContainsKey(key))
            {
                character = characters[key];
            }
            else
            {
                switch (key)
                {
                    case CharacterTypes.Wizard: character = new Wizard(); break;
                    case CharacterTypes.Warrior: character = new Warrior(); break;
                    case CharacterTypes.Healer: character = new Healer(); break;
                }
                characters.Add(key, character);
            }
            return character;
        }
    }
    ```
    - CharacterTypes.cs
    ```c#
    public enum CharacterTypes
    {
        Wizard,
        Warrior,
        Healer
    }
    ```
    - Program.cs
    ```c#
    List<CharacterTypes> characters = new List<CharacterTypes>
    {
        CharacterTypes.Wizard,
        CharacterTypes.Warrior,
        CharacterTypes.Healer,
        CharacterTypes.Warrior

    };

    CharacterFactory factory = new CharacterFactory();
    int damage = 10;
    // For each character use a flyweight object
    foreach (var c in characters)
    {
        damage++;
        Character character = factory.GetCharacter(c);
        character.Display(damage);
    }
    //The Wizard has 50 health points and 11 of damage
    //The Warrior has 70 health points and 12 of damage
    //The Healer has 40 health points and 13 of damage
    //The Warrior has 70 health points and 14 of damage
    ```
## Proxy

- The Proxy design pattern provides a surrogate or placeholder for another object to control access to it.
- Elements:
    1. **Proxy:** maintains a reference that lets the proxy access the real subject. Proxy may refer to a Subject if the RealSubject and Subject interfaces are the same.
        - Provides an interface identical to Subject's so that a proxy can be substituted for for the real subject.
        - Controls access to the real subject and may be responsible for creating and deleting it.
        - Remote proxies are responsible for encoding a request and its arguments and for sending the encoded request to the real subject in a different address space.
        - Virtual proxies may cache additional information about the real subject so that they can postpone accessing it. For example, the ImageProxy from the Motivation caches the real images's extent.
        - Protection proxies check that the caller has the access permissions required to perform a request.
    2. **Subject:** defines the common interface for RealSubject and Proxy so that a Proxy can be used anywhere a RealSubject is expected.
    3. **RealSubject:** defines the real object that the proxy represents.
- Real life example:  
    - Proxy pattern for a Math object represented by a MathProxy object.
    - IMath.cs (Subject interface)
    ```c#
    public interface IMath
    {
        double Add(double x, double y);
        double Sub(double x, double y);
        double Mul(double x, double y);
        double Div(double x, double y);
    }
    ```
    - Math.cs (RealSubject)
    ```c#
    public class Math : IMath
    {
        public double Add(double x, double y) => x + y;

        public double Div(double x, double y) => x / y;

        public double Mul(double x, double y) => x * y;

        public double Sub(double x, double y) => x - y;
    }
    ```
    - MathProxy.cs (Proxy)
    ```c#
     public class MathProxy : IMath
    {
        private Math math = new Math();

        public double Add(double x, double y)
        {
            return math.Add(x, y);
        }

        public double Div(double x, double y)
        {
            return math.Div(x, y);
        }

        public double Mul(double x, double y)
        {
            return math.Mul(x, y);
        }

        public double Sub(double x, double y)
        {
            return math.Sub(x, y);
        }
    }
    ```
    - Program.cs
    ```c#
    class Program
    {
        static void Main(string[] args)
        {
            MathProxy proxy = new MathProxy();
            // Do the math
            Console.WriteLine("4 + 2 = " + proxy.Add(4, 2));
            Console.WriteLine("4 - 2 = " + proxy.Sub(4, 2));
            Console.WriteLine("4 * 2 = " + proxy.Mul(4, 2));
            Console.WriteLine("4 / 2 = " + proxy.Div(4, 2));
        }
    }
    //4 + 2 = 6
    //4 - 2 = 2
    //4 * 2 = 8
    //4 / 2 = 2
    ```
## Iterator

- The Iterator design pattern provides a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
- Elements:
    1. **Iterator:** defines an interface for accessing and traversing elements.
    2. **ConcreteIterator:** implements the Iterator interface.
        - Keeps track of the current position in the traversal of the aggregate.
    3. **Aggregate:** 
        - Defines an interface for creating an Iterator object.
    4. **Concrete aggregate:**
        - Implements the Iterator creation interface to return an instance of the proper ConcreteIterator
- Indexers:
    - Indexers allow instances of a class or struct to be indexed just like arrays. The indexed value can be set or retrieved without explicitly specifying a type or instance member. Indexers resemble properties except that their accessors take parameters.

    - The following example defines a generic class with simple get and set accessor methods to assign and retrieve values. The Program class creates an instance of this class for storing strings:
    - 
- Real life example
    -  Iterate over a collection of items and skip a specific number of items each iteration.
    - Item.cs
    ```c#
    public class Item
    {
        string name;
        // Constructor
        public Item(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get { return name; }
        }
    }
    ```
    - IAbstractCollection.cs (Aggregate)
    ```c#
    public interface IAbstractCollection
    {
        Iterator CreateIterator();
    }
    ```
    - IAbstractIterator.cs (Iterator)
    ```c#
    public interface IAbstractIterator
    {
        Item First();
        Item Next();
        bool IsDone { get; }
        Item CurrentItem { get; }
    }
    ```
    - Iterator.cs (ConcreteIterator)
    ```c#
    public class Iterator : IAbstractIterator
    {
        Collection collection;
        int current = 0;
        int step = 1;
        // Constructor
        public Iterator(Collection collection)
        {
            this.collection = collection;
        }
        // Gets first item
        public Item First()
        {
            current = 0;
            return collection[current] as Item;
        }
        // Gets next item
        public Item Next()
        {
            current += step;
            if (!IsDone)
                return collection[current] as Item;
            else
                return null;
        }
        // Gets or sets stepsize
        public int Step
        {
            get { return step; }
            set { step = value; }
        }
        // Gets current iterator item
        public Item CurrentItem
        {
            get { return collection[current] as Item; }
        }
        // Gets whether iteration is complete
        public bool IsDone
        {
            get { return current >= collection.Count; }
        }
    }
    ```
    - Collection.cs (ConcreteAggregate)
    ```c#
    public class Collection : IAbstractCollection
    {
        List<Item> items = new List<Item>();
        public Iterator CreateIterator()
        {
            return new Iterator(this);
        }
        // Gets item count
        public int Count
        {
            get { return items.Count; }
        }
        // Indexer
        public Item this[int index]
        {
            get { return items[index]; }
            set { items.Add(value); }
        }
    }
    ```
## Observer 

- Defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
- Elements
    1. **Subject:** knows its observers. Any number of Observer objects may observe a subject. Provides an interface for attaching and detaching Observer objects.
    2. **Concrete subject:** stores state of interest to ConcreteObserver. Sends a notification to its observers when its state changes.
    3. **Observer:** defines an updating interface for objects that should be notified of changes in a subject.
    4. **ConcreteObserver:** maintains a reference to a ConcreteSubject object. Stores state that should stay consistent with the subject's. Implements the Observer updating interface to keep its state consistent with the subject's
- Real life example
    - Registered investors are notified every time a stock changes value.
    - Stock.cs (The 'Subject' abstract class)
    ```c#
    public abstract class Stock
    {
        private string symbol;
        private double price;
        private List<IInvestor> investors = new List<IInvestor>();
        // Constructor
        public Stock(string symbol, double price)
        {
            this.symbol = symbol;
            this.price = price;
        }
        public void Attach(IInvestor investor)
        {
            investors.Add(investor);
        }
        public void Detach(IInvestor investor)
        {
            investors.Remove(investor);
        }
        public void Notify()
        {
            foreach (IInvestor investor in investors)
            {
                investor.Update(this);
            }
            Console.WriteLine("");
        }
        // Gets or sets the price
        public double Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    Notify();
                }
            }
        }
        // Gets the symbol
        public string Symbol
        {
            get { return symbol; }
        }

    }
    - IBM.cs (Concrete subject class)
    ```c#
     public class IBM : Stock
    {
        public IBM(string symbol, double price)
           : base(symbol, price)
        {
        }
    }
    ```
    - IInvestor.cs (Observer interface)
    ```c#
    public interface IInvestor
    {
        void Update(Stock stock);
    }
    ```
    - Investor.cs (Concrete observer)
    ```c#
     public class Investor : IInvestor
    {
        private string name;
        private Stock stock;
        // Constructor
        public Investor(string name)
        {
            this.name = name;
        }
        public void Update(Stock stock)
        {
            Console.WriteLine("Notified {0} of {1}'s " +
                "change to {2:C}", name, stock.Symbol, stock.Price);
        }
        // Gets or sets the stock
        public Stock Stock
        {
            get { return stock; }
            set { stock = value; }
        }
    }
    ```
    - Program.cs
    ```c#
    IBM ibm = new IBM("IBM", 120.00);
    ibm.Attach(new Investor("Sorros"));
    ibm.Attach(new Investor("Berkshire"));
    // Fluctuating prices will notify investors
    ibm.Price = 120.10;
    ibm.Price = 121.00;
    ibm.Price = 120.50;
    ibm.Price = 120.75;
    //Notified Sorros of IBM's change to $120.10
    //Notified Berkshire of IBM's change to $120.10

    //Notified Sorros of IBM's change to $121.00
    //Notified Berkshire of IBM's change to $121.00

    //Notified Sorros of IBM's change to $120.50
    //Notified Berkshire of IBM's change to $120.50

    //Notified Sorros of IBM's change to $120.75
    //Notified Berkshire of IBM's change to $120.75
    ```
## Strategy
-  Defines a family of algorithms, encapsulate each one, and make them interchangeable. This pattern lets the algorithm vary independently from clients that use it.

- Elements:
    1. **Strategy:** declares an interface common to all supported algorithms. Context uses this interface to call the algorithm defined by a ConcreteStrategy.
    2. **Concrete strategy:** implements the algorithm using the Strategy interface.
    3. **Context:** is configured with a ConcreteStrategy object. Maintains a reference to a Strategy object may define an interface that lets Strategy access its data.
- Real life example:
    - Strategy pattern which encapsulates sorting algorithms in the form of sorting objects. This allows clients to dynamically change sorting strategies including Quicksort, Shellsort, and Mergesort.
    - SortStrategy.cs (Strategy abstract class)
    ```c#
    public abstract class SortStrategy
    {
        public abstract void Sort(List<string> list);
    }
    ```
    - Quicksort.cs (Concrete strategy)
    ```c#
    public class QuickSort : SortStrategy
    {
        public override void Sort(List<string> list)
        {
            list.Sort();  // Default is Quicksort
            Console.WriteLine("QuickSorted list ");
        }
    }
    ```
    - ReverseSort.cs (Concrete strategy)
    ```c#
    public class ReverseSort : SortStrategy
    {
        public override void Sort(List<string> list)
        {
            list.Reverse();
            Console.WriteLine("ReverseSort list ");
        }
    }
    ```
    - SortedList.cs (The context class)
    ```c#
    public class SortedList
    {
        private List<string> list = new List<string>();
        private SortStrategy sortstrategy;
        public void SetSortStrategy(SortStrategy sortstrategy)
        {
            this.sortstrategy = sortstrategy;
        }
        public void Add(string name)
        {
            list.Add(name);
        }
        public void Sort()
        {
            sortstrategy.Sort(list);
            // Iterate over list and display results
            foreach (string name in list)
            {
                Console.WriteLine(" " + name);
            }
            Console.WriteLine();
        }
    }
    ```
    - Program.cs
    ```c#
    SortedList studentRecords = new SortedList();
    studentRecords.Add("Samual");
    studentRecords.Add("Jimmy");
    studentRecords.Add("Sandra");
    studentRecords.Add("Vivek");
    studentRecords.Add("Anna");
    studentRecords.SetSortStrategy(new QuickSort());
    studentRecords.Sort();
    studentRecords.SetSortStrategy(new ReverseSort());
    studentRecords.Sort();
    //QuickSorted list 
    //Anna
    //Jimmy
    //Samual
    //Sandra
    //Vivek

    //ReverseSort list 
    //Vivek
    //Sandra
    //Samual
    //Jimmy
    //Anna
    ```

