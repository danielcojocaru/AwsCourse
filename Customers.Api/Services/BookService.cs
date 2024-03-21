using Customers.Api.Contracts.Data;
using Customers.Api.Domain;
using Customers.Api.Mapping;
using Customers.Api.Repositories;

namespace Customers.Api.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repo;

    public BookService(IBookRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Create(Book book)
    {
        BookDto bookDto = book.ToDto();
        bool response = await _repo.Create(bookDto);
        return response;
    }

    public async Task<bool> Update(Book book)
    {
        BookDto bookDto = book.ToDto();
        bool response = await _repo.Update(bookDto);
        return response;
    }

    public async Task<bool> Delete(string isbnNumber)
    {
        bool response = await _repo.Delete(isbnNumber);
        return response;
    }

    public async Task<Book?> Get(string isbnNumber)
    {
        BookDto? bookDto = await _repo.Get(isbnNumber);
        if (bookDto is null)
        {
            return null;
        }

        Book book = bookDto.ToDomain();
        return book;
    }
}
