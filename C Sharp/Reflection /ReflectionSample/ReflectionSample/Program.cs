using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReflectionSample
{
    class Program
    {
        private static string _typeFromConfiguration = "ReflectionSample.Alien";

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

            var createdInstance = Activator.CreateInstance(typeof(List<Person>));
            Console.WriteLine(createdInstance.GetType());//System.Collections.Generic.List`1[ReflectionSample.Person]

            //var createdResult = Activator.CreateInstance(typeof(Result<>)); //this fails
            var openResultType = typeof(Result<>);
            var closedResultType = openResultType.MakeGenericType(typeof(Person));
            var createdResult = Activator.CreateInstance(closedResultType);

            Console.WriteLine(createdResult.GetType());//ReflectionSample.Result`1[ReflectionSample.Person]

            var methodInfo = closedResultType.GetMethod("AlterAndReturnValue");
            Console.WriteLine(methodInfo);

            var genericMethodInfo = methodInfo.MakeGenericMethod(typeof(Employee));
            genericMethodInfo.Invoke(createdResult, new object[] { new Employee() });

        }

        public static void ManipulateObjectsWithReflection()
        {
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
            //var iTalkInstance = Activator.CreateInstance(actualTypeFromConfiguration) as ITalk;
            //iTalkInstance.Talk("Hello world"); //Alien talking... Hello world
            dynamic dynamicITalkInstance = Activator.CreateInstance(actualTypeFromConfiguration);
            dynamicITalkInstance.Talk("Hello world"); //Alien talking... Hello world

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

            Console.WriteLine(personForManipulation);

            var talkMethod = personForManipulation.GetType().GetMethod("Talk");
            talkMethod.Invoke(personForManipulation, new[] { "Hola" });

            personForManipulation.GetType().InvokeMember("Yell", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
                null, personForManipulation, new[] { "Something to yell" });
        }

        public static void InspectingMetadata()
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

            foreach (var method in oneTypeFromCurrentAssembly.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
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
