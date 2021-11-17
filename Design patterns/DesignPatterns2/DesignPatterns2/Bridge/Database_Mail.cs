using System;
namespace DesignPatterns2.Bridge
{
    public class Database_Mail : IMessage
    {
        public void Send()
        {
            Console.WriteLine("Mail send from database");
        }
    }
}
