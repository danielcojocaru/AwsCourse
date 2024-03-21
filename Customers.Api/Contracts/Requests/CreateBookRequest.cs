namespace Customers.Api.Contracts.Requests;

public class CreateBookRequest : BookRequestBase
{ }

public class UpdateBookRequest : BookRequestBase
{ }

public class BookRequestBase
{
    public string IsbnNumber { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Author { get; init; } = default!;
    public int PublicationYear { get; init; } = default!;
}
