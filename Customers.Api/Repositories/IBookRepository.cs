using Customers.Api.Contracts.Data;

namespace Customers.Api.Repositories;

public interface IBookRepository
{
    Task<bool> Create(BookDto book);
    Task<bool> Update(BookDto book);
    Task<bool> Delete(string isbnNumber);
    Task<BookDto?> Get(string isbnNumber);
    //Task<List<BookDto>> GetByAuthor(string author);
}
