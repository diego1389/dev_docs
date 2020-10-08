namespace ACME.Maintenance.Domain
{
    public class OrderItem
    {
        public Part Part { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }
}