using InMemoryCacheExample.Models;
using InMemoryCacheExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCacheExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = bookService.GetBooks();
        return Ok(books);
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] Book newBook)
    {
        bookService.AddBook(newBook);
        return Ok();
    }

    [HttpDelete("clear")]
    public IActionResult ClearCache()
    {
        bookService.ClearCache();
        return Ok();
    }

}
