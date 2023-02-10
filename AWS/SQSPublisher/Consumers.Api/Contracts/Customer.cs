namespace Consumers.Api.Services
{
    public class Customer
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }

        internal CustomerMessage ToCustomerMessage()
        {
            var customerMessage = new CustomerMessage
            {
                Name = this.Name,
                LastName = this.LastName
            };

            return customerMessage;
        }
    }

    public class CustomerMessage
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
}