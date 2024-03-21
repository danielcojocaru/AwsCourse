namespace Customers.Api.Domain;

public class Book
{
    public string IsbnNumber { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Author { get; init; } = default!;
    public int PublicationYear { get; init; } = default!;
}
