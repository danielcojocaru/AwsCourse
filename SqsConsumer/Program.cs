using Amazon.SQS;
using Amazon.SQS.Model;

CancellationTokenSource cts = new();
AmazonSQSClient sqsClient = new();
GetQueueUrlResponse queueUrlResponse = await sqsClient.GetQueueUrlAsync("sqsCustomers");
ReceiveMessageRequest receiveMessagesRequest = new()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MaxNumberOfMessages = 10,
    AttributeNames = ["All"],
    MessageAttributeNames = ["All"],
};

while (!cts.IsCancellationRequested)
{
    ReceiveMessageResponse response = await sqsClient
        .ReceiveMessageAsync(receiveMessagesRequest, cts.Token);

    Console.WriteLine($"Received {response.Messages.Count} messages.");

    foreach (Message message in response.Messages)
    {
        Console.WriteLine(message.MessageId);
        Console.WriteLine(message.Body);

        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(1000);
}