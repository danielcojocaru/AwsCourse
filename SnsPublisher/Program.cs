using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
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

var snsClient = new AmazonSimpleNotificationServiceClient();
var topic = await snsClient.FindTopicAsync("topicCustomers");
var publishRequest = new PublishRequest()
{
    TopicArn = topic.TopicArn,
    Message = JsonSerializer.Serialize(customerCreated),
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

var response = await snsClient.PublishAsync(publishRequest);