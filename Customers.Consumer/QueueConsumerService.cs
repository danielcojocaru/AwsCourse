
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly QueueSettings _settings;
    private readonly IMediator _mediator;
    private readonly ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(
        IAmazonSQS sqs,
        IOptions<QueueSettings> settings,
        IMediator mediator,
        ILogger<QueueConsumerService> logger)
    {
        _sqs = sqs;
        _settings = settings.Value;
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        GetQueueUrlResponse queueUrlResponse = await _sqs.GetQueueUrlAsync(_settings.Name, stoppingToken);
        ReceiveMessageRequest receiveMessagesRequest = new()
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MaxNumberOfMessages = 1,
            AttributeNames = ["All"],
            MessageAttributeNames = ["All"],
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            ReceiveMessageResponse response = await _sqs
                .ReceiveMessageAsync(receiveMessagesRequest, stoppingToken);

            Console.WriteLine($"Received {response.Messages.Count} messages.");

            foreach (Message message in response.Messages)
            {
                string messageType = message.MessageAttributes["MessageType"].StringValue;
                Type? type = Type.GetType($"Customers.Consumer.Messages.{messageType}");

                if (type is null)
                {
                    _logger.LogWarning($"Unknown message type: {messageType}.");
                }
                else
                {
                    try
                    {
                        var typedMessage = (IRequest)JsonSerializer.Deserialize(message.Body, type)!;

                        await _mediator.Send(typedMessage, stoppingToken);
                        await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Message failed durring processing for messageType {messageType}.");
                    }

                }
            }

            await Task.Delay(1000);
        }
    }
}
