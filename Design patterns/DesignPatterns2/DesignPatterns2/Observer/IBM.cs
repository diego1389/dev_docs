using System;
namespace DesignPatterns2.Observer
{
    public class IBM : Stock
    {
        public IBM(string symbol, double price)
           : base(symbol, price)
        {
        }
    }
}
