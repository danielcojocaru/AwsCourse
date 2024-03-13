using Amazon.SQS.Model;

namespace Customers.Api.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessage<T>(T message);
}
