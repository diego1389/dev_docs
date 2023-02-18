using System;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CustomerConsumer.Api.Messages;
using MediatR;
using Microsoft.Extensions.Options;

namespace CustomerConsumer.Api
{
    public class QueueConsumerService : BackgroundService
    {
        private readonly IAmazonSQS _sqs;
        private readonly IOptions<QueueSettings> _queueSettings;
        private readonly IMediator _mediator;
        private readonly ILogger<QueueConsumerService> _logger;

        public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings, IMediator mediator, ILogger<QueueConsumerService> logger)
        {
            _sqs = sqs;
            _queueSettings = queueSettings;
            _mediator = mediator;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);
            var receivedMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                AttributeNames = new List<string> { "All"},
                MessageAttributeNames = new List<string> { "All"},
                MaxNumberOfMessages = 1
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqs.ReceiveMessageAsync(receivedMessageRequest, stoppingToken);
                foreach (var message in response.Messages)
                {
                    var messageType = message.MessageAttributes["MessageType"].StringValue;
                    var type = Type.GetType($"CustomerConsumer.Api.Messages.{messageType}");
                    if(type is null)
                    {
                        _logger.LogWarning("Unknown message type: {MessageType}", messageType);
                        continue;
                    }
           
                    try
                    {
                        var typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type);
                        await _mediator.Send(typedMessage, stoppingToken);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError( ex, "Message failed");
                        continue;
                    }

                    //await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                    //    switch (messageType)
                    //    {
                    //        case nameof(CustomerCreated):
                    //            var created = JsonSerializer.Deserialize<CustomerCreated>(message.Body);
                    //            break;
                    //    }

                    await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);

                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

