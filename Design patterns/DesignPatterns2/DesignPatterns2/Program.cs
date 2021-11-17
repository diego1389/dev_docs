using System;
using DesignPatterns2.AbstractFactory;
using DesignPatterns2.Bridge;

namespace DesignPatterns2
{
    #region abstractFactory
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
    class Program
    {
        static void Main(string[] args)
        {
            MailSendBridge mb = new MailSendBridge();
            CSharp_Mail csProvider = new CSharp_Mail();
            VB_Mail vbProvider = new VB_Mail();
            mb.SendFrom(csProvider);
            mb.SendFrom(vbProvider);
        }
    }
    #endregion
}
