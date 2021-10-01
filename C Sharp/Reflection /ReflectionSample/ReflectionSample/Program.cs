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
