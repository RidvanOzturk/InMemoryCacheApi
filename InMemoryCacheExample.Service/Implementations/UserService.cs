using Dapper;
using DapperCacheExample.Data;
using InMemoryCacheExample.Service.Contracts;
using System.Diagnostics;

namespace InMemoryCacheExample.Service.Implementations;

public class UserService(DapperContext context) : IUserService
{
    public async Task<int> GetUserCountAsync()
    {
        using var conn = context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Users");

        stopwatch.Stop();

        Console.WriteLine($"Toplam kullanıcı: {count}");
        Console.WriteLine($"DB sorgusu süresi: {stopwatch.ElapsedMilliseconds}ms");

        return count;
    }
}