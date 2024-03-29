﻿using Amazon.SQS;
using Amazon.SQS.Model;
using Common;
using System.Text.Json;

CustomerCreated customerCreated = new()
{
    Id = Guid.NewGuid(),
    Email = "daniel.cojocaru.ing@gmail.com",
    FullName = "Daniel Cojocaru",
    DateOfBirth = new DateTime(1988, 1, 1),
    GitHubUsername = "danielcojocaru",
};

var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync("sqsCustomers");
var sendMessageRequest = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customerCreated),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        {
            "MessageType", new MessageAttributeValue()
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated),
            }
        }
    }
};
var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();