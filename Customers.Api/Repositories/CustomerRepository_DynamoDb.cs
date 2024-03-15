using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;
using System.Net;
using System.Text.Json;

namespace Customers.Api.Repositories;

public class CustomerRepository_DynamoDb : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private const string _tableName = "tblCustomers";

    public CustomerRepository_DynamoDb(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public Task<bool> CreateAsync(CustomerDto customer)
        => CreateOrUpdate(customer);

    public Task<bool> UpdateAsync(CustomerDto customer)
        => CreateOrUpdate(customer);

    private async Task<bool> CreateOrUpdate(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttribute = Document.FromJson(customerAsJson).ToAttributeMap();

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttribute,
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

    public Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
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
