using Customers.Api.Domain;

namespace Customers.Api.Services;

public interface IBookService
{
    Task<bool> Create(Book book);
    Task<bool> Update(Book book);
    Task<bool> Delete(string isbnNumber);
    Task<Book?> Get(string isbnNumber);
    Task<List<Book>> GetByAuthor(string author, string? title = null);
}
