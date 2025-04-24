namespace InMemoryCacheExample.Service.Caching;

//ask anyway
public static class MemoryCacheKeyStore
{

    private static readonly HashSet<string> _keys = new();

    public static void Add(string key)
    {
        _keys.Add(key);
    }

    public static void Remove(string key)
    {
        _keys.Remove(key);
    }

    public static List<string> GetKeys()
    {
        return _keys.ToList();
    }

    public static void Clear()
    {
        _keys.Clear();
    }
}
