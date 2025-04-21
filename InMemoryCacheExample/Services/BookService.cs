using InMemoryCacheExample.Models;
using Microsoft.Extensions.Caching.Memory;
using InMemoryCacheExample.Settings;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace InMemoryCacheExample.Services;

public class BookService(IMemoryCache memoryCache, IOptions<CacheSettings> settings) : IBookService
{
    private readonly List<Book> fakeBooks = new()
{
    new Book(1, "Game of Thrones ", "George R. R. Martin"),
    new Book(2, "A Young Girl's Diary", "Sigmund Freud"),
    new Book(3, "1984", "George Orwell")
};
    public List<Book> GetBooks()
    {
        const string cacheKey = "bookList";

        if (memoryCache.TryGetValue(cacheKey, out List<Book> cachedBooks))
        {
            return cachedBooks;
        }

        var booksFromSource = fakeBooks;

        var options = new MemoryCacheEntryOptions()
       .SetAbsoluteExpiration(TimeSpan.FromSeconds(settings.Value.BookCacheSeconds));

        memoryCache.Set(cacheKey, booksFromSource, options);

        return booksFromSource;
    }
}
