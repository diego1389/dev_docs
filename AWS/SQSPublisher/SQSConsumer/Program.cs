﻿using Amazon.SQS;
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