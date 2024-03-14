using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Api.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly AwsSettings _awsSettings;
    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<AwsSettings> awsSettings)
    {
        _sqs = sqs;
        _awsSettings = awsSettings.Value;
    }

    public async Task<SendMessageResponse> SendMessage<T>(T message)
    {
        SendMessageRequest sendMessageRequest = new()
        {
            QueueUrl = await GetQueueUrl(),
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                {
                    "MessageType", new MessageAttributeValue()
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name,
                    }
                }
            }
        };

        SendMessageResponse response = await _sqs.SendMessageAsync(sendMessageRequest);
        return response;
    }

    private async Task<string> GetQueueUrl()
    {
        if (_queueUrl is null)
        {
            GetQueueUrlResponse response = await _sqs.GetQueueUrlAsync(_awsSettings.QueueName);
            _queueUrl = response.QueueUrl;

        }
        return _queueUrl;
    }
}
