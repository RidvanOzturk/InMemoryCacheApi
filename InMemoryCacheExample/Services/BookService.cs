using InMemoryCacheExample.Models;
using Microsoft.Extensions.Caching.Memory;
using InMemoryCacheExample.Settings;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace InMemoryCacheExample.Services;

public class BookService(IMemoryCache memoryCache, IOptions<CacheSettings> settings) : IBookService
{
    private const string BookListCacheKey = "bookList";
    private static readonly List<Book> fakeBooks = new()
    {
        new Book(1, "Game of Thrones ", "George R. R. Martin"),
        new Book(2, "A Young Girl's Diary", "Sigmund Freud"),
        new Book(3, "1984", "George Orwell")
    };
    public List<Book> GetBooks()
    {

        if (memoryCache.TryGetValue(BookListCacheKey, out List<Book> cachedBooks))
        {
            return cachedBooks;
        }

        var booksFromSource = fakeBooks;
        var options = new MemoryCacheEntryOptions()
       .SetAbsoluteExpiration(TimeSpan.FromSeconds(settings.Value.BookCacheSeconds));
        memoryCache.Set(BookListCacheKey, booksFromSource, options);
        return booksFromSource;
    }
    public void ClearCache()
    {
        memoryCache.Remove(BookListCacheKey);
    }
    public void AddBook(Book newBook)
    {
        fakeBooks.Add(newBook);
        memoryCache.Remove(BookListCacheKey);
    }

}
