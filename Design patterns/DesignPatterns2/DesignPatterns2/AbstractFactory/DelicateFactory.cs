using System;
namespace DesignPatterns2.AbstractFactory
{
    public class DelicateFactory : PcknDelvFactory
    {
        public override DeliveryDocument CreateDeliveryDocument()
        {
            return new Courier();
        }

        public override Packaging CreatePackaging()
        {
            return new ShockProofPackaging();
        }
    }
}
