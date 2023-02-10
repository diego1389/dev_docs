using System;
using Consumers.Api.Messaging;

namespace Consumers.Api.Services
{
	public class CustomerService
	{
		private readonly ISqsMessenger _sqsMessenger;

		public CustomerService(ISqsMessenger sqsMessenger)
		{
			_sqsMessenger = sqsMessenger;
		}

		public async Task<bool> CreateAsync()
		{
			var customer = new Customer
			{
				Name = "Diego",
				LastName = "Guillen"
			};

			CustomerMessage customerMessage = customer.ToCustomerMessage();

			await _sqsMessenger.SendMessageAsync(customerMessage);
			return true;
		}
	}
}

