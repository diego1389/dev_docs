## Simple Queue Service

- Go to SQS and click on Create Queue. 
- Configure the queue type: standard or FIFO (standard doesn't guarantee the message will not be repeated nor the order but it is the recommended approach).
- Configure queue's visibility timeout, message retention period, delivery delay, maximum message size, receive message wait time. 
- Enable encryption. 
- You can go to the customers queue and click on Send/Receive messages to send and receive messages through the AWS console UI.

### Create a publisher through C#
- Install AWSSDK.SQS
- Send a message using C#:
- CustomerCreated.cs
    ```c#
    using System;
    namespace SQSPublisher
    {
        public class CustomerCreated
        {
            public required Guid Id { get; set; }
            public required string FullName { get; set; }
            public required string Email { get; set; }
            public required string GitHubUsername { get; set; }
            public required DateTime DateOfBirth { get; set; }
        }
    }
    ```
- Program.cs
    ```c#
    using System.Text.Json;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using SQSPublisher;

    var sqsClient = new AmazonSQSClient();

    var customer = new CustomerCreated
    {
        Id = Guid.NewGuid(),
        Email = "dalguillen3089@gmail.com",
        FullName = "Diego Guillen",
        DateOfBirth = new DateTime(1989, 6, 30),
        GitHubUsername = "diego3089"
    };

    var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

    var sendMessageRequest = new SendMessageRequest
    {
        QueueUrl = queueUrlResponse.QueueUrl,
        MessageBody = JsonSerializer.Serialize(customer)
    };

    var response = await sqsClient.SendMessageAsync(sendMessageRequest);
    Console.WriteLine();
    ```
### Creating a message consumer:
- SQS does not delete the messages automatically after they are consumed, you have to specify that.
- Program.cs
    ```c#
    using Amazon.SQS;
    using Amazon.SQS.Model;

    var cts = new CancellationTokenSource();


    var sqsClient = new AmazonSQSClient();

    var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

    var receiveMessageRequest = new ReceiveMessageRequest
    {
        QueueUrl = queueUrlResponse.QueueUrl
    };

    while (!cts.IsCancellationRequested)
    {
        var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
        foreach (var message in response.Messages)
        {
            Console.WriteLine($"Message Id: ${message.MessageId}");
            Console.WriteLine($"Message Body: ${message.Body}");
            await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
        }

        await Task.Delay(1000);
    }
    ```
- Publish message from API:
    - ISqsMessenger.cs interface:
    ```c#
     using System;
    using Amazon.SQS.Model;

    namespace Consumers.Api.Messaging
    {
        public interface ISqsMessenger
        {
            Task<SendMessageResponse> SendMessageAsync<T>(T message);
        }
    }
    ```
    - SQSMessenger.cs
    ```c#
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
    ```
    - QueueSettings.cs
    ```c#
    using System;
    namespace Consumers.Api.Messaging
    {
        public class QueueSettings
        {
            public required string QueueName { get; set; }
        }
    }
    ```
    - Inject the MessagingService in the service that you want:
    ```c#
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
    ```