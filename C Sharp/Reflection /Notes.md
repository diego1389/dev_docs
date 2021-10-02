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
    - 