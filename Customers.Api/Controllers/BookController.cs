﻿using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Customers.Api.Domain;
using Customers.Api.Mapping;
using Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
    {
        Book book = request.ToDomain();
        bool response = await _service.Create(book);
        return response ? Ok() : throw new Exception("Could not create book.");
    }

    [HttpPut("book")]
    public async Task<IActionResult> Update([FromBody] UpdateBookRequest request)
    {
        Book book = request.ToDomain();
        bool response = await _service.Update(book);
        return response ? Ok() : throw new Exception("Could not update book.");
    }

    [HttpDelete("book/{isbnNumber}")]
    public async Task<IActionResult> Delete([FromRoute] string isbnNumber)
    {
        bool deleted = await _service.Delete(isbnNumber);
        if (deleted)
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpGet("book/{isbnNumber}")]
    public async Task<IActionResult> Get([FromRoute] string isbnNumber)
    {
        Book? book = await _service.Get(isbnNumber);

        if (book is null)
        {
            return NotFound();
        }

        var bookResponse = book.ToResponse();
        return Ok(bookResponse);
    }

    [HttpGet("book/byAuthor/{author}")]
    public async Task<IActionResult> GetByAuthor([FromRoute] string author, [FromQuery] string? title)
    {
        List<Book> books = await _service.GetByAuthor(author, title);
        List<BookResponse> booksResponse = books.Select(book => book.ToResponse()).ToList();
        return Ok(booksResponse);
    }
}
