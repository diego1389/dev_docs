using System;
namespace DesignPatterns2.AbstractFactory
{
    public class Client
    {
        private Packaging _packaging;
        private DeliveryDocument _deliveryDocument;

        public Client(PcknDelvFactory factory)
        {
            _packaging = factory.CreatePackaging();
            _deliveryDocument = factory.CreateDeliveryDocument();
        }

        public Packaging ClientPackaging
        {
            get { return _packaging;  }
        }

        public DeliveryDocument ClientDocument
        {
            get { return _deliveryDocument; }
        }
    }
}
