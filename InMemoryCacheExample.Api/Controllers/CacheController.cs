using InMemoryCacheExample.Service.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController(IMemoryCache memoryCache) : ControllerBase
{
    [HttpDelete("clear-by-prefix")]
    public IActionResult ClearByPrefix([FromQuery] string prefix)
    {
        //ask
        var keys = MemoryCacheKeyStore.GetKeys()
            .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var key in keys)
        {
            memoryCache.Remove(key);
            MemoryCacheKeyStore.Remove(key);
        }

        return Ok(new { removed = keys });
    }
}
