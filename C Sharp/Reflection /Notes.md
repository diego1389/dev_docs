## Using reflection for insepcting metadata:

- Reflection: Assembly contain modules -> modules contain types and types contain members. 
- Assembly is a collection of types and resourses that are built to work together and form a logical unit of functionality. .Exe and .dll.
- Modules: a portable, executable file, consisting or one or more classes and interfaces. 
- A type is a collection of members, fields that hold data, methods that can be executed, etc. F.e: class, struct, enum, etc.
- Members of type represent the data and behaviour of a type. 
- Reflection the process by which a computer program can observe and modify its own structure and behavior.
- System.Reflection namespace. Dynamically creating an instance of a type, binding the type to an existing objects or getting the type from an existing object. 
- Dependency injection containers, calling private or protected methods, fields, properties, serialization, type inspector applications, code analysis tools. 
- Reflection is relatively slow.
- Reflection is error-prone.
- Type is an abstract class that derives from MemberInfo.
- Files can also be from assemblies that have not being loaded yet. First load the external assembly by calling the Load method.
    ```c#
    using System;
    using System.Reflection;

    namespace ReflectionSample
    {
        class Program
        {
            static void Main(string[] args)
            {
                string name = "Diego";
                var stringType = name.GetType();
                //if you don't have an instance you can use typeof
                stringType = typeof(string);
                Console.WriteLine(stringType);

                var currentAssembly = Assembly.GetExecutingAssembly();
                var typesFromCurrentAssembly = currentAssembly.GetTypes();
                foreach (var type in typesFromCurrentAssembly)
                {
                    Console.WriteLine(type.Name);
                    /*
                    ITalk
                    EmployeeMarkerAttribute
                    Employee
                    Alien
                    Person
                    Program
                    */
                }

                var externalAssembly = Assembly.Load("System.Text.Json");
                var oneTypeFromExternalAssembly = externalAssembly.GetType("System.Text.Json.JsonProperty");
                Console.WriteLine(oneTypeFromExternalAssembly);//System.Text.Json.JsonProperty

                var modulesFromExternalAssembly = externalAssembly.GetModules();

                foreach (var module in modulesFromExternalAssembly)
                {
                    Console.WriteLine(module.Name);//System.Text.Json.dll
                }

                Console.ReadLine();
            }
        }
    }
    ```

    - Constructor at the end derives from MemberInfo. This class contains information about the attributes of member and provides access to member metadata.
    - GetMethods() does not retrieve protected methods.
    ```c#
    namespace ReflectionSample
    {
        class Program
        {
            static void Main(string[] args)
            {
                string name = "Diego";
                var stringType = name.GetType();
                //if you don't have an instance you can use typeof
                stringType = typeof(string);
                Console.WriteLine(stringType);

                var currentAssembly = Assembly.GetExecutingAssembly();
                var typesFromCurrentAssembly = currentAssembly.GetTypes();
                var oneTypeFromCurrentAssembly = currentAssembly.GetType("ReflectionSample.Person");
                foreach (var type in typesFromCurrentAssembly)
                {
                    Console.WriteLine(type.Name);
                    /*
                    ITalk
                    EmployeeMarkerAttribute
                    Employee
                    Alien
                    Person
                    Program
                    */
                }

                var externalAssembly = Assembly.Load("System.Text.Json");
                var oneTypeFromExternalAssembly = externalAssembly.GetType("System.Text.Json.JsonProperty");
                Console.WriteLine(oneTypeFromExternalAssembly);//System.Text.Json.JsonProperty

                var modulesFromExternalAssembly = externalAssembly.GetModules();

                foreach (var module in modulesFromExternalAssembly)
                {
                    Console.WriteLine(module.Name);//System.Text.Json.dll
                }

                var oneModuleFromExternalAssembly = externalAssembly.GetModule("System.Text.Json.dll");
                var oneTypeFromModuleFromExternalAssembly = oneModuleFromExternalAssembly.GetType("System.Text.Json.JsonProperty");

                foreach (var constructor in oneTypeFromCurrentAssembly.GetConstructors())
                {
                    Console.WriteLine(constructor);/*
                    Void .ctor()                               
                    Void .ctor(System.String)*/
                }

                foreach (var method in oneTypeFromCurrentAssembly.GetMethods())
                {
                    Console.WriteLine(method);/*
                    System.String get_Name()
                    Void set_Name(System.String)
                    Void Talk(System.String)
                    System.String ToString()
                    System.Type GetType()
                    Boolean Equals(System.Object)
                    Int32 GetHashCode()               
                    */
                }
                Console.ReadLine();
            }
        }
    }
    ```

    - Binding is the process of locating the declaration (the implementation) that corresponds to a uniquely specified type.
        - Early binding: looks for methods and properties and checkes wheter they exist and match at compile time.
        - Late binding: objects are dynamic. The type is decided at runtime. 
        - BindingFlags: used to control binding and how reflection searches.
            - How reflection searches: BindingFlags.Public, BindingFlags.Instance.
            - Control the binding itself: BindingFlagas.GetProperty, BindingFlags.SetField.
            ** BIndingFlags can be combined bitwise.

## Instantiating and manipulating objects

- Invoking constructor:
    - To get the correct constructor we have to pass a types[] list that represents the parameters
    ```c#
    //InspectingMetadata();
    var personType = typeof(Person);
    var personConstructors = personType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    foreach (var constructor in personConstructors)
    {
        Console.WriteLine(constructor);
    }

    var privatePersonConstructor = personType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
        null,
        new Type[] { typeof(string), typeof(int) },
        null);

    var person1 = personConstructors[0].Invoke(null);
    var person2 = personConstructors[1].Invoke(new object[] { "Diego" });
    privatePersonConstructor.Invoke(new object[] { "Diego", 32 });

    var person4 = Activator.CreateInstance("ReflectionSample", "ReflectionSample.Person");
    ```
    To get to the functionality you can cast the object to the interface type. 
    ```c#
    class Program
    {
        private static string _typeFromConfiguration = "ReflectionSample.Alien";

        static void Main(string[] args)
        {
            //InspectingMetadata();
            var personType = typeof(Person);
            var personConstructors = personType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var constructor in personConstructors)
            {
                Console.WriteLine(constructor);
            }

            var privatePersonConstructor = personType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(string), typeof(int) },
                null);

            var person1 = personConstructors[0].Invoke(null);
            var person2 = personConstructors[1].Invoke(new object[] { "Diego" });
            privatePersonConstructor.Invoke(new object[] { "Diego", 32 });

            var person4 = Activator.CreateInstance("ReflectionSample", "ReflectionSample.Person").Unwrap();

            var actualTypeFromConfiguration = Type.GetType(_typeFromConfiguration);
            var iTalkInstance = Activator.CreateInstance(actualTypeFromConfiguration) as ITalk;
            iTalkInstance.Talk("Hello world"); //Alien talking... Hello world

        }
    }
    ```
- You can also use dynamics 
    ```c#
    dynamic dynamicITalkInstance = Activator.CreateInstance(actualTypeFromConfiguration);
    dynamicITalkInstance.Talk("Hello world"); //Alien talking... Hello world
    ```
- Manipulate properties and fields:
    ```c#
    var personForManipulation = Activator.CreateInstance("ReflectionSample",
                "ReflectionSample.Person",
                true,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { "Diego", 32 },
                null,
                null).Unwrap();

    var nameProperty = personForManipulation.GetType().GetProperty("Name");
    nameProperty.SetValue(personForManipulation, "DiegoTest");
    Console.WriteLine(personForManipulation); //DiegoTest 32 initial private field value
    var ageField = personForManipulation.GetType().GetField("age");
    ageField.SetValue(personForManipulation, 45);
    var privateField = personForManipulation.GetType().GetField("_aPrivateField", BindingFlags.NonPublic | BindingFlags.Instance);

    privateField.SetValue(personForManipulation, "updated Private Field Value"); //DiegoTest 45 updated Private Field Value

    Console.WriteLine(personForManipulation);
    ```
    - You can also manipulate them without needing to go to the member types using InvokeMember:
    ```c#
    var personForManipulation = Activator.CreateInstance("ReflectionSample",
        "ReflectionSample.Person",
        true,
        BindingFlags.Instance | BindingFlags.NonPublic,
        null,
        new object[] { "Diego", 32 },
        null,
        null).Unwrap();

    var nameProperty = personForManipulation.GetType().GetProperty("Name");
    nameProperty.SetValue(personForManipulation, "DiegoTest");
    Console.WriteLine(personForManipulation); //DiegoTest 32 initial private field value
    var ageField = personForManipulation.GetType().GetField("age");
    ageField.SetValue(personForManipulation, 45);
    var privateField = personForManipulation.GetType().GetField("_aPrivateField", BindingFlags.NonPublic | BindingFlags.Instance);

    privateField.SetValue(personForManipulation, "updated Private Field Value"); //DiegoTest 45 updated Private Field Value

    personForManipulation.GetType().InvokeMember(
        "Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
        null,
        personForManipulation,
        new[] { "Emma" }
    );

    personForManipulation.GetType().InvokeMember(
        "_aPrivateField", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField,
        null,
        personForManipulation,
        new[] { "second updated Private Field Value" }
    );


    Console.WriteLine(personForManipulation);//Emma 45 second updated Private Field Value
    ```
- Invoking methods:
    ```c#
    var talkMethod = personForManipulation.GetType().GetMethod("Talk");
    talkMethod.Invoke(personForManipulation, new[] { "Hola" });
    ```
- Invoking protected methods:
    ```c#
    personForManipulation.GetType().InvokeMember("Yell", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
        null, personForManipulation, new[] { "Something to yell" });
    ```

## Using reflection with generics

- Generics: let you tailor a method, class, structure, interface to the precise data type it acts upon. 
- With generics you avoid boxing and unboxing. T is replaced at compile time to the appropiate type. 
- Use generics with reflection. 
    ```c#
    static void Main(string[] args)
    {
        //InspectingMetadata();
        //ManipulateObjectsWithReflection();
        var myList = new List<Person>();
        Console.WriteLine(myList.GetType().Name); //List`1 (only one generic type parameter (CLS comply))
        var myDictionary = new Dictionary<string, int>();//System.Collections.Generic.Dictionary`2[System.String, System.Int32]
        Console.WriteLine(myDictionary.GetType());

        var dictionaryType = myDictionary.GetType();
        foreach (var genericTypeArgument in dictionaryType.GenericTypeArguments) //or call method GetGenericArguments()
        {
            Console.WriteLine(genericTypeArgument);/*
            System.String
            System.Int32*/
        }
    }
    ```
    - Create a new class:
        ```c#
        public class Result<T>
        {
            public T Value { get; set; }
            public string Remarks { get; set; }
        }


        ```
    - Creating generic instances:
        ```c#
        var createdInstance = Activator.CreateInstance(typeof(List<Person>));
        Console.WriteLine(createdInstance.GetType());//System.Collections.Generic.List`1[ReflectionSample.Person]

        //var createdResult = Activator.CreateInstance(typeof(Result<>)); //this fails
        var openResultType = typeof(Result<>);
        var closedResultType = openResultType.MakeGenericType(typeof(Person));
        var createdResult = Activator.CreateInstance(closedResultType);

        Console.WriteLine(createdResult.GetType());//ReflectionSample.Result`1[ReflectionSample.Person]
        ```
    - Invoking generic methods:
        ```c#
        var methodInfo = closedResultType.GetMethod("AlterAndReturnValue");
        Console.WriteLine(methodInfo);

        var genericMethodInfo = methodInfo.MakeGenericMethod(typeof(Employee));
        genericMethodInfo.Invoke(createdResult, new object[] { new Employee() });
        /*
        A person is being created.
        Altering value using ReflectionSample.Employee*/
        ```
    - Check IoC example.

## Advanced topics
- Reflection magic library makes it easier to work with reflection.
```c#
var person = new Person("Diego");
person.AsDynamic()._aPrivateField = "Private field";
```