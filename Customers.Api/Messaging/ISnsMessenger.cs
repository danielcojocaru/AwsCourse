using Amazon.SimpleNotificationService.Model;

namespace Customers.Api.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> SendMessage<T>(T message);
}
