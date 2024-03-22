using System.Text.Json.Serialization;

namespace Customers.Api.Contracts.Data;

public class BookDto
{
    public const string NAMESPACE = "BOOK";
    public const string TITLE_JSON = "title";
    public const string AUTHOR_JSON = "author";
    public const string YEAR_JSON = "year";

    [JsonPropertyName(BaseDto.PK_JSON)]
    public string Pk { get; init; } = default!; // "BOOK#<IsbnNumber>"

    [JsonPropertyName(BaseDto.SK_JSON)]
    public string Sk => BaseDto.SK_CONSTANT_VALUE;

    [JsonPropertyName(TITLE_JSON)]
    public string Title { get; init; } = default!;

    [JsonPropertyName(AUTHOR_JSON)]
    public string Author { get; init; } = default!;

    [JsonPropertyName(YEAR_JSON)]
    public int PublicationYear { get; init; } = default!;

    [JsonPropertyName(BaseDto.UPDATED_AT_JSON)]
    public DateTime UpdatedAt { get; set; }
}
