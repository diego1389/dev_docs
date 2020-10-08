
# Behavioral patterns

## Singleton

1. A class designed to only ever have one instance. Used for access to file system or shared network resource.

2. A single class with a private instance and a public static method that provides the only way to reference that instance. It also has a private constructor.

3. At any time, only 0 or 1 instance of singleton class exists in the application.

4. Singleton classes are created without parameters. 

5. Lazy instantiation: created only when anything request it.

6. Singleton classes should be marked as sealed. 

7. A private static field holds the only reference to the instance.

8. A public static method provides access to this field. 

```csharp

//Bad implementation: not thread safe
public sealed class Singleton{
    private static Singleton _instance;

    public static Singleton Instance{
        get{
            Logger.Log("Instance called");
            if(_instance == null)
                _instance = new Singleton();
            return _instance;
        }
    }

    private Singleton(){
        Logger.Log("Constructor invoked");
    }
}
//Better approach with lock (still has some problems   )
public sealed class Singleton{
    private static Singleton _instance;
    private static readonly object padlock = new object();

    public static Singleton Instance{
        get{
            Logger.Log("Instance called");
            lock(padlock){
            if(_instance == null)
                _instance = new Singleton();
            return _instance;
            }

        }
    }

    private Singleton(){
        Logger.Log("Constructor invoked");
    }
} 
```

9. Lazy<T>, provides built-in support for lazy inicialization. Can be use to implement the singleton pattern.
```csharp
//Better approach with lock (still has some problems   )
public sealed class Singleton{
    public static readonly Lazy<Singleton> _lazy = new Lazy<Singleton>(()=> new Singleton());

    public static Singleton Instance{
        get{
            Logger.Log("Instance called");
   
            return _lazy.Value;
        }
    }

    private Singleton(){
        Logger.Log("Constructor invoked");
    }
}
```

10. Considered an anti pattern by some people because its difficult to test due to shared state.

11. Doesnt follow separation of concerns and single responsability.

12. Singleton classes are actual class instances, they can implement interfaces. Static class cannot implement interfaces.

13. Singleton can be passed as parameters and static class do not.

14. Singleton can be assign to variables. 

15. Singleton classes can be serialized, static don't.

17. Singleton behaviour using containers is supported in .Net Core (dependency inversion containers).

```csharp
public override void ConfigureServices(IServiceCollection services)
	{
        services.AddTransient<IProveedorApiEndpoints, ProveedorApiEndpoints>(); //lifetime transiet, a new instance is provided any time a class request it
        services.AddScoped<IRepositoriosEF, RepositoriosEF>(); /*lifetime scoped, the first request gets a new instance and the subsecuent request in that scope share the same instance*/ 
		services.AddSingleton<IConfiguracionDeServicios, ConfiguracionDeServicios>();/*life time single, every request get the same instance, lazy*/
        services.AddSingleton<IConfiguracionDeServicios>(new ConfiguracionDeServicios(_configuration)); //create instance up front, creates it when application starts */
    }

```


## State

1. State: the condition of something variable (linguistic sense).

2. One of the 23 design patterns documented by the Gang of 4.

3. State pattern was developed to overcome two primary design challenges:  How can an object change its behaviour when its internal state changes? and how can state specific behaviors be defined so that states can be added without altering the behavior of existing states?

4. The naive approach uses booleans to track the different states but every time it becomes harder to add different states and harder to manage. It is also difficult to extend.

5. State design patter minimizes conditional complexity.

6. It encapsulates state-specific behaviors within separate state objects.

7. A class delegates the execution of its state-specific behaviors to one state object at a time.

8. Thre main elements of the State pattern:
    1. Context: mantains an instance of a concrete state as the current state.
    2. Abstract state: defines an interface which encapsulates all state-specific behaviors.
    3. Concrete state: a subclass of the abstract state that implements behaviors to a particular state of context.

9. A list of possible states.
10. The conditions for transitioning between those states. 
11. The state its in when initialized, or its initial state.
12. **Abstract state:** 

    - BookingContext is acting as the context for the state pattern. By passing it as a parameter to these methods, we're providing the concrete implementations of  state a reference to this context. Its through this interface that the context will interact with the concrete states.
    
    ```csharp

    public abstract class BookingState
        {
            public abstract void EnterState(BookingContext booking);
            public abstract void Cancel(BookingContext booking);

            public abstract void DatePassed(BookingContext booking);

            public abstract void EnterDetails(BookingContext booking, string attendee, int ticketCount);
        }

    ```

13. **Concrete states:** 

    - Create a separate class for each state and implement the abstract class Booking state. 

14. **Context and state:**

    - Modify the BookingContext
    - Booking context is delegating the responsability for handling the methods and any state-specific behaviors associated with it to an instance of a concrete state, its currentState.  


    ```csharp
     public class BookingContext
    {
        public MainWindow View { get; private set; }
        public string Attendee { get; set; }
        public int TicketCount { get; set; }
        public int BookingID { get; set; }

        private BookingState currentState;

        public void TransitionToState(BookingState state)
        {
            currentState = state;
            currentState.EnterState(this); 
            /*this = current instance of the BookingContext class giving the currentState a reference to the context to use in the method*/
        }

        public BookingContext(MainWindow view)
        {
            View = view;
            TransitionToState(new NewState()); //To start the flow with NewState
        }

        public void SubmitDetails(string attendee, int ticketCount)
        {
            currentState.EnterDetails(this, attendee, ticketCount);
        }

        public void Cancel()
        {
            currentState.Cancel(this);
        }

        public void DatePassed()
        {
            currentState.DatePassed(this); 
        }

        public void ShowState(string stateName)
        {
            View.grdDetails.Visibility = System.Windows.Visibility.Visible;
            View.lblCurrentState.Content = stateName;
            View.lblTicketCount.Content = TicketCount;
            View.lblAttendee.Content = Attendee;
            View.lblBookingID.Content = BookingID;
        }

    }
    ```

15. Implementing the pattern

    - Create the behaviors of the different concrete state classes. (Check state design pattern project).

## Strategy 

1. **Context:** has a reference to a strategy and invokes it. 
2. **IStrategy:** an interface for the given strategy.
3. **Strategy:** a concrete implementation of the strategy.

4. We can select an implementation at runtime based on the user input without having to extend the class. 

```csharp
/*Before the strategy pattern*/

public decimal GetTax()
        {
            var destination = ShippingDetails.DestinationCountry.ToLowerInvariant();

            if(destination == "sweden")
            {
                if (destination == ShippingDetails.OriginCountry.ToLowerInvariant())
                {
                    return TotalPrice * 0.25m;
                }

                return 0;
            }

            if (destination == "us")
            {
                switch (ShippingDetails.DestinationState.ToLowerInvariant())
                {
                    case "la": return TotalPrice * 0.095m;
                    case "ny": return TotalPrice * 0.04m;
                    case "nyc": return TotalPrice * 0.045m;
                    default: return 0m;
                }
            }

            return 0m;
        }
    }

```

5. Create an interface:

```csharp
   public interface ISalesTaxStrategy
    {
        public decimal GetTaxFor(Order order);
    }
```
6. Create strategies that implement that interface 
```csharp
public class SwedenSalesTaxStrategy : ISalesTaxStrategy
{
        public decimal GetTaxFor(Order order)
        {
            var destination = order.ShippingDetails.DestinationCountry.ToLowerInvariant();

            var origin  = order.ShippingDetails.OriginCountry.ToLowerInvariant();

            if (destination == origin)
            {
                return order.TotalPrice * 0.25m;
            }

                #region Tax per item
               
                #endregion

                return 0;
         }

    }

public class USSalesTaxStrategy : ISalesTaxStrategy
    {
        public decimal GetTaxFor(Order order)
        {
            switch (order.ShippingDetails.DestinationState.ToLowerInvariant())
            {
                case "la": return order.TotalPrice * 0.095m;
                case "ny": return order.TotalPrice * 0.04m;
                case "nyc": return order.TotalPrice * 0.045m;
                default: return 0m;
            }
        }
    }

     public class Order
    {
  
        public ShippingStatus ShippingStatus { get; set; } = ShippingStatus.WaitingForPayment;

        public ShippingDetails ShippingDetails { get; set; }

        public ISalesTaxStrategy SalesTaxStrategy { get; set; }

        public decimal GetTax()
        {
            if (SalesTaxStrategy == null) return 0;

            return SalesTaxStrategy.GetTaxFor(this);


        }
    }

    /*Main class*/
     static void Main(string[] args)
        {
            var order = new Order
            {
                ShippingDetails = new ShippingDetails 
                { 
                    OriginCountry = "Sweden",
                    DestinationCountry = "Sweden"
                }
            };

            var destination = order.ShippingDetails.DestinationCountry.ToLowerInvariant();

            if(destination == "sweden")
            {
                order.SalesTaxStrategy = new SwedenSalesTaxStrategy();
            }else if(destination == "us")
            {
                order.SalesTaxStrategy = new USSalesTaxStrategy();
            }
            
            order.LineItems.Add(new Item("CSHARP_SMORGASBORD", "C# Smorgasbord", 100m, ItemType.Literature), 1);
            order.LineItems.Add(new Item("CONSULTING", "Building a website", 100m, ItemType.Service), 1);

            Console.WriteLine(order.GetTax());
        }
```
7. An alternative (pass an implementation directly to the method as an optional parameter):
```csharp


    Console.WriteLine(order.GetTax());
    Console.WriteLine(order.GetTax(new SwedenSalesTaxStrategy()));

/*Order.cs*/

  public ISalesTaxStrategy SalesTaxStrategy { get; set; }

        public decimal GetTax(ISalesTaxStrategy salesTaxStrategy = default)
        {

            var strategy = salesTaxStrategy ?? SalesTaxStrategy;

            if (strategy == null) return 0;

            return strategy.GetTaxFor(this);


        }

```

8. Invoice example:

```csharp
 public interface IInvoiceStrategy
    {
        public void Generate(Order order);
    }

    public abstract class InvoiceStrategy : IInvoiceStrategy
    {
        public abstract void Generate(Order order);

        public string GenerateTextInvoice(Order order)
        {
            var invoice = $"INVOICE DATE: {DateTimeOffset.Now}{Environment.NewLine}";

            invoice += $"ID|NAME|PRICE|QUANTITY{Environment.NewLine}";

            foreach (var item in order.LineItems)
            {
                invoice += $"{item.Key.Id}|{item.Key.Name}|{item.Key.Price}|{item.Value}{Environment.NewLine}";
            }

            invoice += Environment.NewLine + Environment.NewLine;

            var tax = order.GetTax();
            var total = order.TotalPrice + tax;

            invoice += $"TAX TOTAL: {tax}{Environment.NewLine}";
            invoice += $"TOTAL: {total}{Environment.NewLine}";

            return invoice;
        }
    }

    public class FileInvoiceStrategy : InvoiceStrategy
    {
        public override void Generate(Order order)
        {
            using (var stream = new StreamWriter($"invoice_{Guid.NewGuid()}.txt"))
            {
                stream.Write(GenerateTextInvoice(order));

                stream.Flush();
            }
        }
    }

/*Implement the interface because you dont need the GenerateText*/
      public class PrintOnDemandInvoiceStrategy : IInvoiceStrategy
    {
        public void Generate(Order order)
        {
            using (var client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(order);

                client.BaseAddress = new Uri("https://pluralsight.com");

                client.PostAsync("/print-on-demand", new StringContent(content));
            }
        }
    }

    /*Order.cs*/
    public class Order
    {
        public Dictionary<Item, int> LineItems { get; } = new Dictionary<Item, int>();

        public ISalesTaxStrategy SalesTaxStrategy { get; set; }

        public IInvoiceStrategy InvoiceStrategy { get; set; }

        public decimal GetTax(ISalesTaxStrategy salesTaxStrategy = default)
        {

            var strategy = salesTaxStrategy ?? SalesTaxStrategy;

            if (strategy == null) return 0;

            return strategy.GetTaxFor(this);


        }

        public void FinalizeOrder()
        {
            if (SelectedPayments.Any(x => x.PaymentProvider == PaymentProvider.Invoice) &&
               AmountDue > 0 &&
               ShippingStatus == ShippingStatus.WaitingForPayment)
            {
                InvoiceStrategy.Generate(this);

                ShippingStatus = ShippingStatus.ReadyForShippment;
            }
            else if (AmountDue > 0)
            {
                throw new Exception("Unable to finalize order");
            }
        }
    }

    /*Main method*/

    static void Main(string[] args)
        {
            var order = new Order
            {
                ShippingDetails = new ShippingDetails 
                { 
                    OriginCountry = "Sweden",
                    DestinationCountry = "Sweden"
                }
            };

            var destination = order.ShippingDetails.DestinationCountry.ToLowerInvariant();

            if(destination == "sweden")
            {
                order.SalesTaxStrategy = new SwedenSalesTaxStrategy();
            }else if(destination == "us")
            {
                order.SalesTaxStrategy = new USSalesTaxStrategy();
            }
            
            order.LineItems.Add(new Item("CSHARP_SMORGASBORD", "C# Smorgasbord", 100m, ItemType.Literature), 1);
            order.LineItems.Add(new Item("CONSULTING", "Building a website", 100m, ItemType.Service), 1);


            Console.WriteLine(order.GetTax());
            Console.WriteLine(order.GetTax(new SwedenSalesTaxStrategy()));

            order.SelectedPayments.Add(new Payment()
            {
                PaymentProvider = PaymentProvider.Invoice
            });

            order.InvoiceStrategy = new FileInvoiceStrategy();
            order.FinalizeOrder();
        }
```

9. Whenever we can pass an interface that allows us to override functionality, we can pass a strategy.

```csharp
  class Program
    {
        static void Main(string[] args)
        {
            var orders = new[] {
                new Order
                {
                    ShippingDetails = new ShippingDetails
                    {
                        OriginCountry = "Sweden"
                    }
                },
                new Order
                {
                    ShippingDetails = new ShippingDetails
                    {
                        OriginCountry = "USA"
                    }
                },
                new Order
                {
                    ShippingDetails = new ShippingDetails
                    {
                        OriginCountry = "Sweden"
                    }
                },
                new Order
                {
                    ShippingDetails = new ShippingDetails
                    {
                        OriginCountry = "USA"
                    }
                },
                new Order
                {
                    ShippingDetails = new ShippingDetails
                    {
                        OriginCountry = "Singapore"
                    }
                }
            };

            Print(orders);

            Console.WriteLine();
            Console.WriteLine("Sorting..");
            Console.WriteLine();

            /// TODO: Sort array

            Array.Sort(orders, new OrderAmountComparer());

            Print(orders);
        }

        static void Print(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                Console.WriteLine(order.ShippingDetails.OriginCountry);
            }
        }
    }

    public class OrderAmountComparer : IComparer<Order>
    {
        public int Compare(Order x, Order y)
        {
            var xTotal = x.TotalPrice;
            var yTotal = y.TotalPrice;
            if (xTotal == yTotal)
            {
                return 0;
            }
            else if (xTotal > yTotal)
            {
                return 1;
            }

            return -1;
        }
    }
    public class OrderOriginComparer : IComparer<Order>
    {
        public int Compare(Order x, Order y)
        {
            var xDest = x.ShippingDetails.OriginCountry.ToLowerInvariant();
            var yDest = y.ShippingDetails.OriginCountry.ToLowerInvariant();
            if (xDest == yDest)
            {
                return 0;
            }
            else if(xDest[0] > yDest[0])
            {
                return 1;
            }

            return -1;
        }
    }
```
10. Strategy pattern allows us to replace funcionality at runtime based on behaviors for our users.

## Mediator

1. Avoids objects refering to each other.
2. We create a central mediator object, it mantains the references to the objects in our network and keep them in sync at all times. 
3. Components:
    1. **Mediator:** I define communication between colleagues. Abstract based class. 
    2. **Colleague:** I communicate only with the mediator. Abstract based class that represents a related collection of objects. It references only its mediator.  
    3. **Concrete mediator:** I implement communication between colleagues. Inherits and implements defined in the mediator. 
    4. **Concrete colleague:** I receive messages from the mediator. Different types of subclasses that inherit from the abstract colleague base class and defines specific behaviors. 
4. The mediator knows about the colleagues and each colleague knows about its single mediator.
```csharp
   public abstract class Mediator
    {
        public abstract void Send(string message, Colleague colleague);
    }

     public abstract class Colleague
    {
        protected Mediator mediator;

        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual void Send(string message)
        {
            this.mediator.Send(message, this);
        }

        public abstract void HandleNotification(string message);
    }

    public class Colleague1 : Colleague
    {
        public Colleague1(Mediator mediator) : base(mediator)
        {
        }

        public override void HandleNotification(string message)
        {
            Console.WriteLine($"Colleague1 receives notification message: {message}");
        }
    }

     public class Colleague2 : Colleague
    {
        public Colleague2(Mediator mediator) : base(mediator)
        {
        }

        public override void HandleNotification(string message)
        {
            Console.WriteLine($"Colleague2 receives notification message: {message}");
        }
    }

    public class ConcreteMediator : Mediator
    {
        public Colleague1 Colleague1 { get; set; }
        public Colleague2 Colleague2 { get; set; }

         public override void Send(string message, Colleague colleague)
        {
            if (colleague == this.Colleague1)
            {
                this.Colleague2.HandleNotification(message);
            }
            else
            {
                this.Colleague1.HandleNotification(message);
            }
        }

        static void Main(string[] args)
        {
            var mediator = new ConcreteMediator();
            var c1 = new Colleague1(mediator);
            var c2 = new Colleague2(mediator);
            mediator.Colleague1 = c1;
            mediator.Colleague2 = c2;

            c1.Send("Hello, World! (from c1)");
            c2.Send("hi, there! (from c2)");
        }

    /*
    Colleague2 receives notification message: Hello, World! (from c1)
    Colleague1 receives notification message: hi, there! (from c2)
*/

```
5. To work  with n colleagues: 

```csharp
 public class ConcreteMediator : Mediator
    {
        private List<Colleague> colleagues = new List<Colleague>();

        public void Register(Colleague colleague)
        {
            colleague.SetMediator(this);
            this.colleagues.Add(colleague);
        }

        public T CreateColleague<T>() where T : Colleague, new()
        {
            var c = new T();
            c.SetMediator(this);
            this.colleagues.Add(c);
            return c;
        }

        public override void Send(string message, Colleague colleague)
        {
            this.colleagues.Where(c => c != colleague).ToList().ForEach(c => c.HandleNotification(message));
        }
    }

     public class Colleague1 : Colleague
    {

        public override void HandleNotification(string message)
        {
            Console.WriteLine($"Colleague1 receives notification message: {message}");
        }
    }

       public class Colleague2 : Colleague
    {

        public override void HandleNotification(string message)
        {
            Console.WriteLine($"Colleague2 receives notification message: {message}");
        }
    }

 private static void StructuralExample()
        {
            var mediator = new ConcreteMediator();

            var c1 = mediator.CreateColleague<Colleague1>();
            var c2 = mediator.CreateColleague<Colleague2>();

            c1.Send("Hello, World! (from c1)");
            c2.Send("hi, there! (from c2)");
        }
```

6. Real world example : Chat

```csharp
/*Mediator: */
public abstract class Chatroom
    {
        public abstract void Register(TeamMember member);
        public abstract void Send(string from, string message);
        public abstract void SendTo<T>(string from, string message) where T : TeamMember;
    }

/*Abstract colleague*/
 public abstract class TeamMember
    {
        private Chatroom chatroom;

        public TeamMember(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        internal void SetChatroom(Chatroom chatroom)
        {
            this.chatroom = chatroom;
        }

        public void Send(string message)
        {
            this.chatroom.Send(this.Name, message);
        }

        public virtual void Receive(string from, string message)
        {
            Console.WriteLine($"from {from}: '{message}'");
        }

        public void SendTo<T>(string message) where T : TeamMember
        {
            this.chatroom.SendTo<T>(this.Name, message);
        }
    }

/*Concrete mediator*/
     public class TeamChatroom : Chatroom
    {
        private List<TeamMember> members = new List<TeamMember>();

        public override void Register(TeamMember member)
        {
            member.SetChatroom(this);
            this.members.Add(member);
        }

        public override void Send(string from, string message)
        {
            this.members.ForEach(m => m.Receive(from, message));
        }

        public void RegisterMembers(params TeamMember[] teamMembers)
        {
            foreach (var member in teamMembers)
            {
                this.Register(member);
            }
        }

        public override void SendTo<T>(string from, string message)
        {
            this.members.OfType<T>().ToList().ForEach(m => m.Receive(from, message));
        }
    }

    /*Concrete colleagues*/

    public class Developer : TeamMember
    {
        public Developer(string name) : base(name)
        {
        }

        public override void Receive(string from, string message)
        {
            Console.Write($"{this.Name} ({nameof(Developer)}) has received: ");
            base.Receive(from, message);
        }
    }

      public class Tester : TeamMember
    {
        public Tester(string name) : base(name)
        {
        }

        public override void Receive(string from, string message)
        {
            Console.Write($"{this.Name} ({nameof(Tester)}) has received: ");
            base.Receive(from, message);
        }
    }

     static void Main(string[] args)
        {
          
            var teamChat = new TeamChatroom();

            var steve = new Developer("Steve");
            var justin = new Developer("Justin");
            var jenna = new Developer("Jenna");
            var kim = new Tester("Kim");
            var julia = new Tester("Julia");
            teamChat.RegisterMembers(steve, justin, jenna, kim, julia);

            steve.Send("Hey everyone, we're going to be deploying at 2pm today.");
            kim.Send("Ok, thanks for letting us know.");

            Console.WriteLine();
            steve.SendTo<Developer>("Make sure to execute your unit tests before checking in!");
        }



```

## Command

1. 


# Creational patterns

## Builder

1.  It deals with creating objects. Separate the construction of a complex object from its representation, so that the same construction proccess can create different representation.s.

2. The builder pattern removes any and all construction or initialization code from an object class and abstracts it out to an interface. Concrete classes dont deal with instantiating themselves. That's up to the director class, which controls where and with what data the concrete classes are 

3. The builder pattern in practice:
    1. Defining an object class.
    2. Adding a builder interface.
    3. Creating a concrete builder class.
    4. Implement a director class.
    5. Update to a fluent builder variation. 

4. Each concrete builder is in charge of keeping track of the representation of the complex object it creates, and retreiving that object when queried.

5. The director class handles the actual call to construct the complex object using t he concrete buildert. 

6. Builder is useful when creating complex objects, when object creation needs to be separate from its assembly and when different representations need to be created with finer control.

7. The builder interface needs to be general enough to apply to all the different kinds of concrete builder

    ```csharp
        public interface IFurnitureInventoryBuilder
    {
        void AddTitle();
        void AddDimensions();
        void AddLogistics();

        InventoryReport GetDailyReport(); //you can add this in the interface or in every concrete builder
    }
    ```
8. Implement concrete builder class:
    ```csharp
    public class DailyReportBuilder : IFurnitureInventoryBuilder
    {
        private InventoryReport _report;
        private IEnumerable<FurnitureItem> _items;

        public DailyReportBuilder(IEnumerable<FurnitureItem> items)
        {
            Reset();
            _items = items;
        }

        public void AddDimensions()
        {
            _report.DimensionsSection = string.Join(Environment.NewLine, _items.Select(product 
                => $"Product: {product.Name} \n Price: {product.Price}" +
                $"Heigth:  { product.Height}"));
        }

        public void AddLogistics(DateTime dateTime)
        {
            _report.LogisticsSection = $"Report generated on {dateTime}";
        }

        public void AddTitle()
        {
            _report.TitleSection = "----------Daily Inventory Report----------\n\n";
        }

        public InventoryReport GetDailyReport()
        {
            InventoryReport finishedReport = _report;
            Reset();

            return finishedReport;
        }

        public void Reset()
        {
            _report = new InventoryReport();
        }
    }
    ```
    9. The director class only has one job and it is to execute the objects build steps in a predetermined sequence.
    
    10. Fluent builder:
     ```csharp
      return new StringBuilder()
                .AppendLine(TitleSection)
                .AppendLine(DimensionsSection)
                .AppendLine(LogisticsSection)
                .ToString();  
      ```
    11. To make it work in our example we need every method of the builder concrete class to return an object of its own class. It doesnt use the director

      ```csharp
        public interface IFurnitureInventoryBuilder
            {
                IFurnitureInventoryBuilder AddTitle();
                IFurnitureInventoryBuilder AddDimensions();
                IFurnitureInventoryBuilder AddLogistics(DateTime dateTime);
                InventoryReport GetDailyReport(); //you can add this in the interface or in every concrete builder
            }

            public class DailyReportBuilder : IFurnitureInventoryBuilder
            {
                private InventoryReport _report;
                private IEnumerable<FurnitureItem> _items;

                public DailyReportBuilder(IEnumerable<FurnitureItem> items)
                {
                    Reset();
                    _items = items;
                }

                public IFurnitureInventoryBuilder AddDimensions()
                {
                    _report.DimensionsSection = string.Join(Environment.NewLine, _items.Select(product 
                        => $"Product: {product.Name} \n Price: {product.Price}" +
                        $"Heigth:  { product.Height}"));
                    return this;
                }

                public IFurnitureInventoryBuilder AddLogistics(DateTime dateTime)
                {
                    _report.LogisticsSection = $"Report generated on {dateTime}";
                    return this;
                }

                public IFurnitureInventoryBuilder AddTitle()
                {
                    _report.TitleSection = "----------Daily Inventory Report----------\n\n";
                    return this;
                }

                public InventoryReport GetDailyReport()
                {
                    InventoryReport finishedReport = _report;
                    Reset();

                    return finishedReport;
                }

                public void Reset()
                {
                    _report = new InventoryReport();
                }
            }
            /*--------------------------------------------------*/
            static void Main(string[] args)
            {
                var items = new List<FurnitureItem>
                {
                    new FurnitureItem("Sectional Couch", 55.5, 22.4, 78.6, 35.0),
                    new FurnitureItem("Nightstand", 25.0, 12.4, 20.0, 10.0),
                    new FurnitureItem("Dining Table", 105.0, 35.4, 100.6, 55.5),
                };

                var inventoryBuilder = new DailyReportBuilder(items);
                /*var director = new InventoryBuildDirector(inventoryBuilder);
                director.BuildCompleteReport();
                var directorReport = inventoryBuilder.GetDailyReport();
                Console.WriteLine(directorReport.Debug());*/
                var fluentReport = inventoryBuilder
                                    .AddTitle()
                                    .AddDimensions()
                                    .AddLogistics(DateTime.Now)
                                    .GetDailyReport();
                 Console.WriteLine(fluentReport.Debug());
            }
      ```

      12. If you have a class with bloated constructors full of parameters with a lot of computation to create objects its a good indication to use builder pattern.

      13.  A director doesnt have to know about how an object its built or its internal structure. 

      14. The pattern isolates code for construction and representation. 

      15. The director only has to choose which preset build sequence to use and reuse. 

      16. The builder pattern is focused on object creation in sequential steps, while the factory pattern is concerned with families or groups of objects being created. 

        

## Prototype

1. Client request a clone of an existing object.
2. Cloning login in the object itself.
3. Cuts down on subclassing.
4. We have a prototype interface which is an abstract class that has the blueprint clone method for all our adopting classes. Then we have as  many concrete classes prototypable classes as we need, each one responsible of returning a copy of itself. Then we have a client which make the actual cloning request from an existing object which implements our prototype interface.
5. When object creation, composition and representation needs to be separate from a given system.
6.  Shallow copy: All non static value types of the object are copied to the clone, but reference types are copied only the addresess. So changes to the clone implies changes to the original. 

```csharp
 public abstract class Prototype
    {
        public abstract Prototype ShallowCopy();
        public abstract Prototype DeepCopy();
        public abstract void Debug();
    }

    public class OrderInfo
    {
        public int Id { get; set; }
        public OrderInfo(int id)
        {
            this.Id = id;
        }
    }

    public class FoodOrder : Prototype
    {
        public string CustomerName { get; set; }
        public bool IsDelivery { get; set; }
        public string[] OrderContents { get; set; }
        public OrderInfo Info;

        public FoodOrder(string name, bool delivery, string[] contents, OrderInfo orderInfo)
        {
            this.CustomerName = name;
            this.IsDelivery = delivery;
            this.OrderContents = contents;
            this.Info = orderInfo;
        }

        public override void Debug()
        {
            Console.WriteLine("-----------------Prototype Food Order-----------------");
            Console.WriteLine("\nName: {0} \nDelivery", CustomerName, IsDelivery.ToString());
            Console.WriteLine("\nID: {0}", this.Info.Id);
            Console.WriteLine("Order contents : " + string.Join(",", OrderContents) + "\n");
        }

        public override Prototype ShallowCopy()
        {
            return (Prototype)this.MemberwiseClone();
        }

        public override Prototype DeepCopy()
        {
            FoodOrder cloneOrder = (FoodOrder)this.MemberwiseClone();
            cloneOrder.Info = new OrderInfo(this.Info.Id);
            return cloneOrder;
        }
    }
    /****************The program class**********************/

    static void Main(string[] args)
        {
            Console.WriteLine("Original order:");
            FoodOrder savedOrder = new FoodOrder(
                "Diego", 
                true, 
                new string[] { "pizza", "coke"},
                new OrderInfo(123));

            savedOrder.Debug();
            Console.WriteLine("Cloned order:");
            //FoodOrder clonedOrder = (FoodOrder)savedOrder.ShallowCopy();
            FoodOrder clonedOrder = (FoodOrder)savedOrder.DeepCopy();
            clonedOrder.Debug();


            Console.WriteLine("Order changes:");
            savedOrder.CustomerName = "Jeff";
            savedOrder.Info.Id = 555;
            savedOrder.Debug();
            clonedOrder.Debug();

            Console.Read();

        }
```
7. Prototype manager:

```csharp
public class PrototypeManager {
        private Dictionary<string, Prototype> prototypeLibrary = new Dictionary<string, Prototype>();

        public Prototype this[string key]
        {
            get { return prototypeLibrary[key]; }
            set { prototypeLibrary.Add(key, value); }
        }
    }

    /****************The program class**********************/

    PrototypeManager manager = new PrototypeManager();
    manager["2/3/2020"] = new FoodOrder("Steve", false, new string[] { "chicken" }, new OrderInfo(8912));

    Console.WriteLine("Manager clone: ");
    FoodOrder managerOrder = (FoodOrder)manager["2/3/2020"].DeepCopy();
    managerOrder.Debug();

```

8. Uses when classes to instantiate are specified at run time.
9. When classes have a small, finite combination of states.
10. Implement in those classes that make sense.

## Prototype

