using Amazon.SQS;
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

AmazonSQSClient sqsClient = new();
GetQueueUrlResponse queueUrlResponse = await sqsClient.GetQueueUrlAsync("sqsCustomers");
SendMessageRequest sendMessageRequest = new()
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
SendMessageResponse response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();