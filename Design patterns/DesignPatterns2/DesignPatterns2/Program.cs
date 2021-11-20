using System;
using DesignPatterns2.AbstractFactory;
using DesignPatterns2.Bridge;
using DesignPatterns2.Decorator;
using DesignPatterns2.Facade;
using DesignPatterns2.Prototype;

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
    #endregion
}
