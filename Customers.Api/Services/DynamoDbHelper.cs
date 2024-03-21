using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Text.Json;

namespace Customers.Api.Services;

public class DynamoDbHelper : IDynamoDbHelper
{
    public Dictionary<string, AttributeValue> ConvertToDynamoDbItem<T>(T dto)
    {
        string serialized = JsonSerializer.Serialize(dto);
        var item = Document.FromJson(serialized)
                           .ToAttributeMap();
        return item;
    }

    public T? DeserializeFromDynamoDbItem<T>(Dictionary<string, AttributeValue> item)
    {
        var itemAsJson = Document.FromAttributeMap(item).ToJson();
        T? deserialized = JsonSerializer.Deserialize<T>(itemAsJson);
        return deserialized;
    }
}
