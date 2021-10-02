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

            foreach (var method in oneTypeFromCurrentAssembly.GetMethods(BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic))
            {
                Console.WriteLine(method);/*              
                Void Finalize()
                Boolean Equals(System.Object)
                Int32 GetHashCode()              
                Void set_Name(System.String)
                Void Talk(System.String)
                Void Yell(System.String)
                System.String ToString()
                System.Type GetType()
                System.Object MemberwiseClone()               */
            }
            Console.ReadLine();
        }
    }
}
