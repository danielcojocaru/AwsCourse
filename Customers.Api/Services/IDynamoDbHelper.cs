using Amazon.DynamoDBv2.Model;

namespace Customers.Api.Services;

public interface IDynamoDbHelper
{
    Dictionary<string, AttributeValue> ConvertToDynamoDbItem<T>(T dto);
    T? DeserializeFromDynamoDbItem<T>(Dictionary<string, AttributeValue> item);
}
