using Customers.Api.Contracts.Data;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToDtoMapper
{
    public static CustomerDto ToDto(this Customer customer)
        => new()
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };

    public static BookDto ToDto(this Book book)
        => new()
        {
            Pk = $"{BookDto.NAMESPACE}#{book.IsbnNumber}",
            Author = book.Author,
            PublicationYear = book.PublicationYear,
            Title = book.Title,
        };
}
