using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;
using Customers.Api.Services;
using System.Net;
using System.Text.Json;

namespace Customers.Api.Repositories;

public class CustomerRepository_DynamoDb : ICustomerRepository
{
    private const string _tableName = "tblCustomers";

    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly IDynamoDbHelper _dynamoDbHelper;

    public CustomerRepository_DynamoDb(IAmazonDynamoDB dynamoDb, IDynamoDbHelper dynamoDbHelper)
    {
        _dynamoDb = dynamoDb;
        _dynamoDbHelper = dynamoDbHelper;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttribute = Document.FromJson(customerAsJson).ToAttributeMap();

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttribute,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)",
        };

        var response = await _dynamoDb.PutItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttribute = Document.FromJson(customerAsJson).ToAttributeMap();
        string requestStarted = ":requestStarted";

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttribute,
            ConditionExpression = $"{nameof(CustomerDto.UpdatedAt)} < {requestStarted}",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { requestStarted, new AttributeValue{ S = customer.UpdatedAt.ToString("O")} },
            },
        };

        var response = await _dynamoDb.PutItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var request = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue{ S = id.ToString()} },
                { "sk", new AttributeValue{ S = id.ToString()} },
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        // Don't do this:
        var scanRequest = new ScanRequest
        {
            TableName = _tableName,
        };

        var response = await _dynamoDb.ScanAsync(scanRequest);

        var customers = new List<CustomerDto>();
        foreach (var item in response.Items)
        {
            var json = Document.FromAttributeMap(item).ToJson();
            CustomerDto? customer = JsonSerializer.Deserialize<CustomerDto>(json);
            if (customer is not null)
            {
                customers.Add(customer);
            }
        }

        return customers;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var request = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue{ S = id.ToString()} },
                { "sk", new AttributeValue{ S = id.ToString()} },
            }
        };

        var response = await _dynamoDb.GetItemAsync(request);

        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsJson = Document.FromAttributeMap(response.Item).ToJson();
        CustomerDto? customer = JsonSerializer.Deserialize<CustomerDto>(itemAsJson);
        return customer;
    }
}
