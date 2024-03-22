using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;
using Customers.Api.Services;
using System.Net;
using System.Text.Json;

namespace Customers.Api.Repositories;

public class BookRepository : IBookRepository
{
    private const string _tableName = "bookstore";

    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly IDynamoDbHelper _dynamoDbHelper;
    private readonly ILogger<BookRepository> _logger;

    public BookRepository(IAmazonDynamoDB dynamoDb, IDynamoDbHelper dynamoDbHelper, ILogger<BookRepository> logger)
    {
        _dynamoDb = dynamoDb;
        _dynamoDbHelper = dynamoDbHelper;
        _logger = logger;
    }

    public async Task<bool> Create(BookDto book)
    {
        book.UpdatedAt = DateTime.UtcNow;
        var bookItem = _dynamoDbHelper.ConvertToDynamoDbItem(book);

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = bookItem,
            ConditionExpression = $"attribute_not_exists({BaseDto.PK_JSON})",
        };

        var response = await _dynamoDb.PutItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> Update(BookDto book)
    {
        book.UpdatedAt = DateTime.UtcNow;
        var bookItem = _dynamoDbHelper.ConvertToDynamoDbItem(book);

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = bookItem,
            ConditionExpression = $"{BaseDto.UPDATED_AT_JSON} < :requestStarted",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":requestStarted", new AttributeValue{ S = book.UpdatedAt.ToString("O")} },
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
                { BaseDto.PK_JSON, new AttributeValue{ S = $"{BookDto.NAMESPACE}#{isbnNumber}" } },
                { BaseDto.SK_JSON, new AttributeValue{ S = BaseDto.SK_CONSTANT_VALUE } },
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
                { BaseDto.PK_JSON, new AttributeValue{ S = $"{BookDto.NAMESPACE}#{isbnNumber}" } },
                { BaseDto.SK_JSON, new AttributeValue{ S = BaseDto.SK_CONSTANT_VALUE } },
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

    public async Task<List<BookDto>> GetByAuthor(string author, string? title = null)
    {
        var request = new QueryRequest
        {
            TableName = _tableName,
            IndexName = "author-title-index",
            KeyConditionExpression = $"{BookDto.AUTHOR_JSON} = :vAuthor",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {
                    ":vAuthor", new AttributeValue{ S = author }
                }
            }
        };

        var response = await _dynamoDb.QueryAsync(request);

        List<BookDto> books = ConvertToBooks(response);

        return books;
    }

    private List<BookDto> ConvertToBooks(QueryResponse response)
    {
        List<BookDto> books = new();
        foreach (var item in response.Items)
        {
            BookDto? book = _dynamoDbHelper.DeserializeFromDynamoDbItem<BookDto>(item);
            if (book is not null)
            {
                books.Add(book);
            }
            else
            {
                try
                {
                    string serializedItem = JsonSerializer.Serialize(item);
                    _logger.LogWarning($"Cannot deserialize as {nameof(BookDto)} the DynamoDb Item = {serializedItem}.");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Cannot deserialize as {nameof(BookDto)} or as string the DynamoDb Item = {item}.");
                }
            }
        }

        return books;
    }
}
