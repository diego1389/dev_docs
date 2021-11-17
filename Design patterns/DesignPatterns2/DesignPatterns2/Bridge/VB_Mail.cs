using System;
namespace DesignPatterns2.Bridge
{
    public class VB_Mail : IMessage
    {
        public void Send()
        {
            Console.WriteLine("Mail send from VB code");
        }
    }
}
