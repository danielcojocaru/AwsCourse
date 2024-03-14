namespace Customers.Api.Settings;

public class AwsSettings
{
    public required string QueueName { get; init; }
    public required string TopicName { get; init; }
}
