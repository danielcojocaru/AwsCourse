using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;
using Customers.Api.Services;
using System.Net;

namespace Customers.Api.Repositories;

public class BookRepository : IBookRepository
{
    private const string _tableName = "bookstore";

    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly IDynamoDbHelper _dynamoDbHelper;

    public BookRepository(IAmazonDynamoDB dynamoDb, IDynamoDbHelper dynamoDbHelper)
    {
        _dynamoDb = dynamoDb;
        _dynamoDbHelper = dynamoDbHelper;
    }

    public async Task<bool> Create(BookDto book)
    {
        book.UpdatedAt = DateTime.UtcNow;
        var bookItem = _dynamoDbHelper.ConvertToDynamoDbItem(book);

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = bookItem,
            ConditionExpression = "attribute_not_exists(pk)",
        };

        var response = await _dynamoDb.PutItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> Update(BookDto book)
    {
        book.UpdatedAt = DateTime.UtcNow;
        var bookItem = _dynamoDbHelper.ConvertToDynamoDbItem(book);
        string requestStarted = ":requestStarted";

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = bookItem,
            ConditionExpression = $"u < {requestStarted}",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { requestStarted, new AttributeValue{ S = book.UpdatedAt.ToString("O")} },
            },
        };

        var response = await _dynamoDb.PutItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> Delete(string isbnNumber)
    {
        var request = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue{ S = $"{BookDto.NAMESPACE}#{isbnNumber}" } },
                { "sk", new AttributeValue{ S = BookDto.SK } },
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<BookDto?> Get(string isbnNumber)
    {
        var request = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue{ S = $"{BookDto.NAMESPACE}#{isbnNumber}" } },
                { "sk", new AttributeValue{ S = BookDto.SK } },
            }
        };

        var response = await _dynamoDb.GetItemAsync(request);

        if (response.Item.Count == 0)
        {
            return null;
        }

        BookDto? book = _dynamoDbHelper.DeserializeFromDynamoDbItem<BookDto>(response.Item);
        return book;
    }
}
