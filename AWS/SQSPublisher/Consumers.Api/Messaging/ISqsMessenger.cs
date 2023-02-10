using System;
using Amazon.SQS.Model;

namespace Consumers.Api.Messaging
{
	public interface ISqsMessenger
	{
		Task<SendMessageResponse> SendMessageAsync<T>(T message);
	}
}

