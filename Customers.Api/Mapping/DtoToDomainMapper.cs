using Customers.Api.Contracts.Data;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
        => new()
        {
            Id = customerDto.Id,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };

    public static Book ToDomain(this BookDto bookDto)
        => new()
        {
            IsbnNumber = bookDto.Pk.Substring(BookDto.NAMESPACE.Length + 1),
            Author = bookDto.Author,
            PublicationYear = bookDto.PublicationYear,
            Title = bookDto.Title,
        };
}
