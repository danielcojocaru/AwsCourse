using Customers.Api.Contracts.Responses;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToApiContractMapper
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new()
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };

    public static GetAllCustomersResponse ToResponse(this IEnumerable<Customer> customers)
        => new()
        {
            Customers = customers.Select(x => new CustomerResponse
            {
                Id = x.Id,
                Email = x.Email,
                GitHubUsername = x.GitHubUsername,
                FullName = x.FullName,
                DateOfBirth = x.DateOfBirth
            })
        };

    public static BookResponse ToResponse(this Book book)
        => new()
        {
            IsbnNumber = book.IsbnNumber,
            Author = book.Author,
            PublicationYear = book.PublicationYear,
            Title = book.Title,
        };
}
