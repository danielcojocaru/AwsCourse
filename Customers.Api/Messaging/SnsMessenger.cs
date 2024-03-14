using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Customers.Api.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

public class SnsMessenger : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly AwsSettings _queueSettings;
    private string? _topicUrl;

    public SnsMessenger(
        IAmazonSimpleNotificationService sns,
        IOptions<AwsSettings> queueSettings)
    {
        _sns = sns;
        _queueSettings = queueSettings.Value;
    }

    public async Task<PublishResponse> SendMessage<T>(T message)
    {
        var publishRequest = new PublishRequest()
        {
            TopicArn = await GetTopicUrl(),
            Message = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                {
                    "MessageType", new MessageAttributeValue()
                    {
                        DataType = "String",
                        StringValue =  typeof(T).Name,
                    }
                }
            }
        };

        var response = await _sns.PublishAsync(publishRequest);
        return response;
    }

    private async Task<string> GetTopicUrl()
    {
        if (_topicUrl is null)
        {
            var topic = await _sns.FindTopicAsync(_queueSettings.TopicName);
            _topicUrl = topic.TopicArn;

        }
        return _topicUrl;
    }
}
