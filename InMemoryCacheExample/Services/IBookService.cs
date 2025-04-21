using InMemoryCacheExample.Models;

namespace InMemoryCacheExample.Services;

public interface IBookService
{
    List<Book> GetBooks();
    void ClearCache();
    void AddBook(Book newBook);
}
