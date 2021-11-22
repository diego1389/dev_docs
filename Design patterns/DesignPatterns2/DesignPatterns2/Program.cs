using System;
using System.Collections.Generic;
using DesignPatterns2.AbstractFactory;
using DesignPatterns2.Bridge;
using DesignPatterns2.Decorator;
using DesignPatterns2.Facade;
using DesignPatterns2.FactoryMethod;
using DesignPatterns2.Flyweight;
using DesignPatterns2.IteratorPattern;
using DesignPatterns2.IteratorPattern.Indexers;
using DesignPatterns2.Observer;
using DesignPatterns2.Prototype;
using DesignPatterns2.Proxy;

namespace DesignPatterns2
{
    #region AbstractFactory
    /*
    class Program
    {
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

            //DesignPatterns2.AbstractFactory.StandardPackaging
            //DesignPatterns2.AbstractFactory.Postal
            //------------------
            //DesignPatterns2.AbstractFactory.ShockProofPackaging
            //DesignPatterns2.AbstractFactory.Courier             

        }
    }*/
    #endregion
    #region factoryMethod
    /*class Program
    {
        static void Main(string[] args)
        {

        }
    */
    #endregion
    #region Bridge
    /*class Program
    {
        static void Main(string[] args)
        {
            MailSendBridge mb = new MailSendBridge();
            CSharp_Mail csProvider = new CSharp_Mail();
            VB_Mail vbProvider = new VB_Mail();
            mb.SendFrom(csProvider);
            mb.SendFrom(vbProvider);
        }
    }*/
    #endregion
    #region Prototype
    /*class Program
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
    }*/
    #endregion
    #region Decorator
    /*class Program
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


            //Book ----- 
             //Author: Worley
             //Title: Inside ASP.NET
             //# Copies: 10

            //Video ----- 
             //Director: Spielberg
             //Title: Jaws
             //# Copies: 23
             //Playtime: 92


            //Making video borrowable:

            //Video ----- 
            // Director: Spielberg
             //Title: Jaws
             //# Copies: 21
             //Playtime: 92

             //borrower: Customer #1
             //borrower: Customer #2

    }*/
    #endregion
    #region Facade
    /*class Program
    {
        static void Main(string[] args)
        {
            var mortgage = new Mortgage();
            var customer = new Customer("Diego");
            bool isEligible = mortgage.IsEligible(customer, 170000);

            Console.WriteLine($@"{customer.Name} has been  {(isEligible ? "Approved" : "Rejected")}");

        }
    }*/
    #endregion
    #region Flyweight
    /*class Program
    {
        static void Main(string[] args)
        {
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

        }
    }*/
    #endregion
    #region Proxy
    /*class Program
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
    }*/
    #endregion
    #region Iterator
    /*class Program
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
    }*/
    #endregion
    #region Iterator
    /*class Program
    {
        static void Main(string[] args)
        {
            //Indexers
            //var stringCollection = new SampleCollection<string>();
            //stringCollection[0] = "Hello";
            //stringCollection[99] = "World";

            //Console.WriteLine(stringCollection[99]); //World


            Collection collection = new Collection();
            collection[0] = new Item("Item 0");
            collection[1] = new Item("Item 1");
            collection[2] = new Item("Item 2");
            collection[3] = new Item("Item 3");
            collection[4] = new Item("Item 4");
            collection[5] = new Item("Item 5");
            collection[6] = new Item("Item 6");
            collection[7] = new Item("Item 7");
            collection[8] = new Item("Item 8");
            // Create iterator
            Iterator iterator = collection.CreateIterator();
            // Skip every other item
            iterator.Step = 2;
            Console.WriteLine("Iterating over collection:");
            for (Item item = iterator.First();
                !iterator.IsDone; item = iterator.Next())
            {
                Console.WriteLine(item.Name);
            }
        }
    }*/
    #endregion
    #region FactoryMethod
    /*class Program
    {
        static void Main(string[] args)
        {
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
        }
    }*/
    #endregion
    #region Observer
    class Program
    {
        static void Main(string[] args)
        {
            IBM ibm = new IBM("IBM", 120.00);
            ibm.Attach(new Investor("Sorros"));
            ibm.Attach(new Investor("Berkshire"));
            // Fluctuating prices will notify investors
            ibm.Price = 120.10;
            ibm.Price = 121.00;
            ibm.Price = 120.50;
            ibm.Price = 120.75;
        }
    }
    #endregion
}
