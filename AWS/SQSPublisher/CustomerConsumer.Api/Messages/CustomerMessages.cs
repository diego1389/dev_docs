using System;

namespace CustomerConsumer.Api.Messages
{
	public class CustomerCreated : ISqsMessage
	{
		public required string FirstName { get; init; }
		public required string GitHubUser { get; init; }
		public required string Email { get; init; }
	}

    public class CustomerUpdated : ISqsMessage
    {
        public required string FirstName { get; init; }
        public required string GitHubUser { get; init; }
        public required string Email { get; init; }
    }
}

