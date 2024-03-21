using Customers.Api.Contracts.Requests;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class ApiContractToDomainMapper
{
    public static Customer ToDomain(this CustomerRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            GitHubUsername = request.GitHubUsername,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth
        };

    public static Customer ToDomain(this UpdateCustomerRequest request)
        => new()
        {
            Id = request.Id,
            Email = request.Customer.Email,
            GitHubUsername = request.Customer.GitHubUsername,
            FullName = request.Customer.FullName,
            DateOfBirth = request.Customer.DateOfBirth
        };

    public static Book ToDomain(this BookRequestBase request)
        => new()
        {
            IsbnNumber = request.IsbnNumber,
            Author = request.Author,
            PublicationYear = request.PublicationYear,
            Title = request.Title,
        };
}
