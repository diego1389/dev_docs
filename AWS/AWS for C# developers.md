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

## Implementing the consumer

- Install MediatR library
- Create Messages folder:
- ISqsMessage.cs
    ```c#
    using System;
    using MediatR;

    namespace CustomerConsumer.Api.Messages
    {
        public interface ISqsMessage : IRequest
        {

        }
    }
    ```
- CustomerMessages.cs:
    ```c#
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
    ```
- appSettings.json:
    ```js
    {
    "Queue": {
        "Name": "customers" 
    },
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*"
    }
    ```
- Add Handlers folder:
- CustomerCreatedHandler.cs:
    ```c#
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
                _logger.LogInformation(request.FirstName);
                return Unit.Task;
            }
        }
    }
    ```
- CustomerUpdateHandler.cs
    ```c#
    using System;
    using CustomerConsumer.Api.Messages;
    using MediatR;

    namespace CustomerConsumer.Api.Handlers
    {
        public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
        {
            private readonly ILogger<CustomerUpdatedHandler> _logger;

            public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
            {
                _logger = logger;
            }

            public Task<Unit> Handle(CustomerUpdated request, CancellationToken cancellationToken)
            {
                _logger.LogInformation(request.GitHubUser);
                return Unit.Task;
            }
        }
    }
    ```
- Create the service:
- QueueConsumerService.cs:
    ```c#
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
    ```
- Program.cs
    ```c#
   using Amazon.SQS;
    using CustomerConsumer.Api;
    using MediatR;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
    builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
    builder.Services.AddHostedService<QueueConsumerService>();
    builder.Services.AddMediatR(typeof(Program));
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    //var summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    //app.MapGet("/weatherforecast", async () =>
    //{

    //    //var service = app.Services.GetRequiredService<QueueConsumerService>();
    //    //await service.ExecuteAsync(new CancellationToken());

    //    var forecast =  Enumerable.Range(1, 5).Select(index =>
    //        new WeatherForecast
    //        (
    //            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //            Random.Shared.Next(-20, 55),
    //            summaries[Random.Shared.Next(summaries.Length)]
    //        ))
    //        .ToArray();
    //    return forecast;
    //})
    //.WithName("GetWeatherForecast")
    //.WithOpenApi();

    app.Run();

    //record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    //{
    //    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    //}
    ```
- A dead letter queue to handle messages with errors that return to the queue, failed to be processed and return to the queue again for all eternity. After certain amount of retries the message goes to the dead letter queue.
- Create new queue (customers-dlc), extend retention period. 
- Edit the customers queue -> find Dead-letter queue and select the customers-dlq from the dropdown.
- Redrive dead messages: go to redrive allow policy. 
