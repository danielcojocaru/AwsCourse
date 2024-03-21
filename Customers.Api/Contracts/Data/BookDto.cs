using System.Text.Json.Serialization;

namespace Customers.Api.Contracts.Data;

public class BookDto
{
    public const string NAMESPACE = "BOOK";
    public const string SK = "!";

    [JsonPropertyName("pk")]
    public string Pk { get; init; } = default!; // "BOOK#<IsbnNumber>"

    [JsonPropertyName("sk")]
    public string Sk => SK;

    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;

    [JsonPropertyName("author")]
    public string Author { get; init; } = default!;

    [JsonPropertyName("year")]
    public int PublicationYear { get; init; } = default!;

    [JsonPropertyName("u")]
    public DateTime UpdatedAt { get; set; }
}
