using System;
namespace DesignPatterns2.Bridge
{

    public class CSharp_Mail : IMessage
    {
        public void Send()
        {
            Console.WriteLine("Mail send from C# code");
        }
    }
}
