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