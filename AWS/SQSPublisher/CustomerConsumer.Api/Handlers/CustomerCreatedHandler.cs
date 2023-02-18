using System;
using CustomerConsumer.Api.Messages;
using MediatR;

namespace CustomerConsumer.Api.Handlers
{
    public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedHandler> _logger;

        public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CustomerCreated request, CancellationToken cancellationToken)
        {
            //_logger.LogInformation(request.FirstName);
            throw new Exception("Something broke oops");
            return Unit.Task;
        }
    }
}

