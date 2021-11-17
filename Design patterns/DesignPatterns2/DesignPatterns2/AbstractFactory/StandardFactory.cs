using System;
namespace DesignPatterns2.AbstractFactory
{
    public class StandardFactory : PcknDelvFactory
    {
        public override DeliveryDocument CreateDeliveryDocument()
        {
            return new Postal();
        }

        public override Packaging CreatePackaging()
        {
            return new StandardPackaging();
        }
    }
}
