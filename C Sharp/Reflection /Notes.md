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