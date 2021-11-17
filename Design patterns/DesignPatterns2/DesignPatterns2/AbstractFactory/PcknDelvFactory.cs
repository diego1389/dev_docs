using System;
namespace DesignPatterns2.AbstractFactory
{
    public abstract class PcknDelvFactory
    {
        public abstract Packaging CreatePackaging();
        public abstract DeliveryDocument CreateDeliveryDocument();
    }

    public abstract class Packaging { }
    public class StandardPackaging : Packaging { }
    public class ShockProofPackaging : Packaging { }
    public abstract class DeliveryDocument { }
    public class Postal : DeliveryDocument { }
    public class Courier : DeliveryDocument { }
}
