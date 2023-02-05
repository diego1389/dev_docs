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