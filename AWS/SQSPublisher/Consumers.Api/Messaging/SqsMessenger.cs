using System;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Consumers.Api.Messaging
{
	public class SqsMessenger : ISqsMessenger
	{
		private readonly IAmazonSQS _sqs;
		private readonly IOptions<QueueSettings> _queeSettings;
		private string? _queueUrl;

		public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
		{
			_queeSettings = queueSettings;
			_sqs = sqs;
		}

		private async Task<string> GetQueueUrlAsync()
		{
			if (_queueUrl is not null)
				return _queueUrl;

            var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queeSettings.Value.QueueName);
			_queueUrl = queueUrlResponse.QueueUrl;
			return _queueUrl;
        }

        public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
        {
			
			var sendMessageRequest = new SendMessageRequest
			{
				QueueUrl = await GetQueueUrlAsync(),
				MessageBody = JsonSerializer.Serialize(message),
				MessageAttributes = new Dictionary<string, MessageAttributeValue>
				{
					{
						"MessageType", new MessageAttributeValue
						{
							DataType = "String",
							StringValue = typeof(T).Name
						}
					}
				}
			};

			return await _sqs.SendMessageAsync(sendMessageRequest);
        }
    }
}

