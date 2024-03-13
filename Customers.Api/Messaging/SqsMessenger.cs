using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Api.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly QueueSettings _queueSettings;
    private string? queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings.Value;
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
        if (queueUrl is null)
        {
            GetQueueUrlResponse response = await _sqs.GetQueueUrlAsync(_queueSettings.Name);
            queueUrl = response.QueueUrl;

        }
        return queueUrl;
    }
}
