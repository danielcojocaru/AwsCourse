namespace Customers.Api.Contracts.Responses;

public class BookResponse
{
    public string IsbnNumber { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Author { get; init; } = default!;
    public int PublicationYear { get; init; } = default!;
}
