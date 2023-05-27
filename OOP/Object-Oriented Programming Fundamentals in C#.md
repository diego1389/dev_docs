## Introduction

- The main objective of a **class** is to provide a definition for the appropiate type of object. **The class provides the definition of the object type.**
- An **object** is an instance of a class, using the *new* keyword.
- The object variable (customer) holds the state of the object (retains the values of its properties).
    ```cs
    Customer customer = new Customer();
    ```
- **Objects** exist only at runtime. When the application terminates the object is gone but the **classes** still exists.

- **Business objects** normally refers to a class, classes specifically defined for solving a business problem. 
- A class is a cookie cutter with properties and actions to create the cookiess (objects). Each object defines its own values for the properties. 
- **Entity:** anything from the real world that is significant enough to be represented in an application. 

- *Example:*
    - Entity: customer
    - Class: 
        1. Customer
            1. Properties:
                1. Last Name
                2. First Name
            2. Methods
                1. Go on adventure
    - Object:
        1. Bilbo Baggins
        2. Frodo Baggins 

- **Object-oriented programming:** An approach for designing applications that are: flexible, natural, well-cared, testable by focusing on objects that interact cleanly with one another. 
    1. Start by **identifying the classes** from the requirements.  
        1. Represents business entities.
        2. Defines properties (data)
        3. Defines methods (actions/behaviours)
    2. **Separating responsabilities** into classes. If each class has a singular purpose its easier to write, test and later find that class. 
    3. **Establishing relationships** between the classes. 
    4. **Leveraging reuse:** extracting commonality among a set of classes into a separate class. 
- OOP is an iterative process. 

## Identifying classes from requirements

1. Analize the business problem
2. Start with the nouns to identify appropiate classes
    1. Manage business, residential, government and educator types of customers = **customer**
        1. *Properties:* Name, Email address, Home address, work address.
        2. *Methods:* Validate(), Retrieve(), Save()
    2. Manage both current products and Consolidated System's. = **product**
        1. *Properties:* Product name, description, current price. 
        2. *Methods:* Validate(), Retrieve(), Save()
    3. Accept orders online and via call center = **orders**
        1. *Properties:* Customer, Order date, shipping address.
        2. *Methods:* Validate(), Retrieve(), Save()
    4. We need an **order item**
        1. *Properties:* Product, Quantity, Purchase Price
        2. *Methods:* Validate(), Retrieve()
3. Define appropiate members
4. Pillars of OOP
    1. Abstraction
        1. Focus on the features appropiate for our purposes. Simplify the reality and think of a customer just in terms of a contact. 
        2. Defining appropiate classes focusing on what it's important for a purpose. 
        3. The appropiate abstractions depends on the requirements of the application. 
        4. **Abstraction as the process of defining classes by simplifying reality, ignoring extraneous details.** 
    2. Encapsulation
        1. It's a **tecnique to hide or encapsulate the data or implementation details within a class, thus hiding complexity.** 
        2. The data is exposed through getters and setters, the operations are exposed through methods. 
        3. The class data and code can only be accessed through the class's interface. 
        4. Make it posible to build large, full-feature applications, by breaking complex operations into encapsulated units. 
        5. **Data hiding** protects the data, allow for authorization before getting the data, allows for validation before setting the data. 
        6. **Implementation hiding** helps manage  complexity, only the class needs to know the implementation, code is easy to mantain and test, implementation can be changed without impacting the application. 
- It's a good idea to consider how **time** will affect your classes. The costumer may change the address in the future? What happens with the order when the product's price change (add property purchase price).

## Building entity classes

- Most applications have at least 3 layers:
    1. **User interface**
        1. Forms or pages displayed to the user.
        2. Logic to control UI elements
    2. **Business logic**
        1. Logic to perform business operations
    3. **Data access layer**
        1. Logic to retrieve data from the db.
        2. Logic to save data to the db..
    4. Most applications have a **Common layer**
        1. Common code used in all layers like logging or sending messages. 
- Each layer goes in a different project in VS. 

- Create a class library for the business logic and place it under the ACM solution (Check in Code folder for the project).

- If you want the class to be called only in the currect project (only in the Business Logic layer you can use internal). 

- If you don't need any logic into the getter and setter use auto-implemented properties. Otherwise, use the full property syntax.

- propg + Tab + Tab = A property with a getter and a private setter. 
    ```cs
    public int CustomerId { get; private set; }
    ```
- A property with no setter for the full name to follow the especification format:
    ```cs
    public string FullName
        {
            get
            {
                return FirstName + "," + LastName;
            }
        }
    ```
- The diference between a private setter is that it can be changed inside the class but with no setter (like FullName) it cannot be set at all. 

```cs
/*Public to call it from other by any external component*/
    public class Customer 
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string EmailAddress { get; set; }

        public int CustomerId { get; private set; }

        public string FullName
        {
            get
            {
                return FirstName + "," + LastName;
            }
        }


    }
```

## Testing the class

- Write an automated code test (a separate project).
- It must have a reference to the Business Logic project. 
- Automated test creates an instance of the class 
*Check defensive coding on C#*

- Add a test project and write the following test method:
    ```cs
    using ACM.BusinessLogic;

    namespace ACM.BusinessLogicTest
    {
    [TestClass]
        public class CustomerTest
        {
            [TestMethod]
            public void FullNameTestValid()
            {
                //-- Arrange 
                Customer customer = new Customer();
                customer.FirstName = "Bilbo";
                customer.LastName = "Baggins";

                string expected = "Baggins, Bilbo";
                //-- Act
                string actual = customer.FullName;
                //--Assert
                Assert.AreEqual(expected, actual);
            }
        }
    }
    ```
- Objects are reference types. 

- Static: declares a member that belongs to the class itself. 

- Running all the tests helps to check in the future if the changes break the new code. 

- The contract of the class is the interface (properties and methods "promised"). Once the code is deployed you should not remove properties and methods, but the contract can be extended to include new elements in the class.

- A constructor is a method tha it's executed each time an instance of the class is created. 
- The *this* keyword means the current instance of the class. 
- If someone place an order in Detroit and Paris at 10 am those are actually different hours. To compare them you can use DateTimeOffset.

## Separating responsabilities

- Separating concerns minimizes coupling, maximizes cohesion, simplifies maintenance and improves testability. 
- **Coupling is the degree to which classes are dependent on each other. The fewer dependencies the easier it is to mantain, test and update.** 
- If there are depencies in one class consider moving them to another class. 
- **Cohesion is a measure of how related everything in a class is to the purpose of the class. If there are properties or methods that are not related to the purpose of the class they should be moved to another class.** 
- When there is low coupling there is a reduced propability that changes to one class will adversely affect another class, making maintenance easier. 
- Low coupling makes it easy to test a class because it has minimum dependencies to another classes.
- When there is high cohesion there is a higher propability that a feature request will affect only a small number of classes. High cohesion makes the class well defined and complete. 
- Classes must have low coupling and high cohesion. 
- For convention the class in charge of retrieving and saving data to a db is called _Repository (Customer Repository).
- Repository pattern. 
- When checking Assert.AreEqual with reference types they have to reference the same object (not different objects with the same properties). To run the test you have to compare each of the properties. 
- Design patterns as common practices for defining appropiate classes an their associated relationships. 

## Estabilishin relationships

- Define how the objects work together to perform the operations of the application. 
- Three types of relationships in OOP:  
    - **Collaborative ("uses as"):** One class uses another class. The customer repository uses the Customer class to populate it and to serialize it. 
        - When an object from one class uses one or more instances of another(otherwise unrelated) class.
        - Objects provide services to each other. 
        - Also when it takes an instance of another class as a parameter. 
    - **Composition ("has a"):** an object has another. The order has a customer, an address and order items.
        - Agregation: special type of composition whereby the component parts do not exist except as part of the composition (like OrderItem class).
        - Composite objects often contain references of to their component objects as properties. 
        - Constructors should ensure that an object is in valid state when it is constructed.
        - To initialize both constructors without repeating the line of code you can make the default constructor to call the other constructor. This tecnique is called *constructor chaining*.
            ```cs
            /*Constructors*/
            public Customer() : this(0)
            {

            }

            public Customer(int customerId)
            {
                this.CustomerId = customerId;
                AddressList = new List<Address>();
            }
            ```
        - The constructor of the Customer class constructs a set of addresses anytime the Customer is constructed. 
        - Another way to establish a composition relationship is using **Id properties** instead of object properties (an integer CustomerId in the Order class instead of a Customer object).
             1. It minimizes coupling because the order class does not need to directly reference the customer or address classes. 
             2. More efficient because the order class does not need to load all the Customer information. 
             3. Save is simple, it doesn't need the saving of customer or address information. 
            ```cs
            public class Order
            {
                /*Properties*/
                public DateTimeOffset? OrderDate { get; set; }
                public int OrderId { get; private set; }
                public List<OrderItem> OrderItems { get; set; }
                public int CustomerId { get; set; }
                public int ShippingAddressId { get; set; }
            }
            ```
            4. You need to add extra classes for shipping and view/update of the Order class because it doesn't have Customer and Address information anymore and you will need them. Create OrderDisplay class and OrderDisplayItem classes, they will take the responsability to create a displayable order (with Customer and Addresses information). Instead of using a reference to the Customer class, it simply has its own property for the Customer name, and instead of the order having a reference to the product class, it simply has the product name as one of its properties. 

    - **Inheritance ("is a"):** A business type of costumer is a customer. 
        - The class above is the base class or parent class. 
        - Inheritance allows to build a class that inherits the members of another class so you can define a more specific type. 
        - Levering reuse. 
        - The children class inherit all the properties and behaviors. 
        - C# does not allow multiple inheritance but it does allow defining any number of inheritance levels (inheritance chain).

## Leveraging reuse

- Involves extracting the commonality.
- Add, building reusable components. 
- Build the set of functionality once and reuse it wherever need it. 
- Test it once with unit tests once and retain the test for regression testing as neeeded.
- Update it once and re test it.
- If you extend or enhance the functionality you only have to do it once. 
- The secret of successful reuse is to have the code in one place. 
- All the derived classes inherits all the properties and methods of the classes it inherits from. 
- A derived class can provide a custom implementation of a base class member such a ToString().
- Write override + space and intellisense shows a list of overridable methods of a certain class.
    ```cs
    public override string ToString()
    {
        return OrderDate + " (" + OrderId + ")";
    }
    ```
- **Polymorphism:** a single method can behave differently depending of the type of object that calls it. 
- Good practice not to delete anything from db. Just change status to active.
- An abstract class cannot be instantiated, you can't use the new keyword, only to be used as a base class. 
- Sealed: a class that cannot be extended through inheritance.
- You can add a Validate method in the base class and the override it in the children. By default methods from an abstract class are not overridable.
    1. Use the *virtual* keyword if you want to provide the option for the method to be overriden. It has its own implementation. 
    2. Use the *abstract* keyword if the method must be overriden. It doesnt have its own implementation (it has no statement body). 
    ```cs
    public abstract class EntityBase
    {
        public EntityStateOption EntityState { get; set; }
        public bool HasChanges { get; set; }
        public bool IsNew { get; private set; }
        private bool _isValid;

        public bool IsValid
        {
            get {
                return Validate();
            }
        }

        public abstract bool Validate(); 
        /*Product*/

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
                return false;
            if (CurrentPrice == null)
                return false;
            return true; ;
        }
    }
    ```
## Building reusable components

- Sometimes you need a reusable library of general purpose classes that is not related with business logic code (logging, etc).
- Each component a different project: User interface layer, business logic, data access layer and general purpose library (Common).
- If the class has no object level state there is no reason to have an instance (use static).
- Use static classes for general purpose project. 
- To create an extension method of the .Net framework (like Trim or uppercase) you only have to add the this keyword before the first parameter.
    1. It must reside in a static class
    ```cs
     public static class StringHandler
    {
        public static string InsertSpaces(this string source)
        {
            ...
        }
    }
    //to call it later in the Product classs:
     private string _productName;

        public string ProductName
        {
            get {
                return _productName.InsertSpaces();
            }
            set {
                _productName = value;
            }
        }
    ```
- To decide between extension method or static method:
    1. Is the primary parameter an instance?
    2. Does the method operates on that instance?
    3. Is it desirable to appear on intellisense? 
    - If the answer is yes to all it could be an extension method.  

## Understanding interfaces

- An interface is a boundary across which two separate components of a computing system exchange information:
    1. The boundary between the user and an applicatios is called the User Interface.
    2. The boundary between an application that provides services and the clients requesting those services is called a Web API (Web Application Programming Interface).
    3.  In OOP the boundary between each class and the rest of the application is called a class interface.
- The **public** properties and methods of a class define the *implicit* class interface.
- Interfaces show as connectors. 
- An interface is only the definition of the properties and methods. It does not include any implementation, the actual code is encapsulated. 
- **Explicit interface:** a separate type that you can create using the **interface** keyword. Unlike a class it does not provide an implementation. 
    1. It can be reused in any class that wants to implement the interface. 
    2. Code can access the class through its *implicit interface* (public properties and methods) and through its *explicit interface* (Customer implements ILoggable so you can call the Log() method from Customer).7
- .Net framework provides with a number of built-in interfaces like IComparable and IDisposable. 
- Interfaces as roles you want to implement. 
- You have the common in a separate project but you want to "use" the Log() method of Customer and Product classes but cannot cast them because there would be a circular reference:
    ```cs
    namespace ACM.BusinessLogic.ACM.Common
    {
        public class LoggingService
        {
            public static void WriteToFile(List<object> changedObjects)
            {
                foreach (var item in changedObjects)
                {
                    Console.WriteLine(item.);/*no Log() method and cannot cast it*/
                }
            }
        }
    }
    ```
    1. It is better to implement an interface in each class. 
        ```cs
        public interface ILoggable
        {
            string Log();
        }
        ```
    2. Members of an interface does not require an access modifier, they are automatically public. 
    3. No implementation of the members (only the signatures). 
    4. The implemented methods must be public, nonstatic and have the same name and signature as the interface member. 
- **Interface based polymorphism:** 
    ```cs
    public static void WriteToFile(List<ILoggable> changedObjects)
    {
        foreach (var item in changedObjects)
        {
            Console.WriteLine(item.Log());
        }
    }
    ```