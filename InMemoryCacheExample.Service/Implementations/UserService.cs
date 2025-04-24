using Dapper;
using DapperCacheExample.Data;
using InMemoryCacheExample.Data.Entities;
using InMemoryCacheExample.Service.Caching;
using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.DTOs;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace InMemoryCacheExample.Service.Implementations;

public class UserService(DapperContext context, IMemoryCache memoryCache) : IUserService
{
    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
    {
        using var conn = context.CreateConnection();
        var users = await conn.QueryAsync<User>("SELECT * FROM Users");
        return users.Select(user => new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Fullname = user.Fullname
        });
    }


    public async Task<UserResponseDTO?> GetByIdAsync(int id)
    {
        var cacheKey = $"user-id-{id}";

        if (memoryCache.TryGetValue(cacheKey, out UserResponseDTO cachedUser))
        {
            Console.WriteLine("there is cache");
            return cachedUser;
        }
        //to check
        Console.WriteLine("there is no cache, going to db");

        using var conn = context.CreateConnection();
        var user = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });

        if (user is null)
            return null;

        var model = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Fullname = user.Fullname
        };

        memoryCache.Set(cacheKey, model, TimeSpan.FromMinutes(10));
        MemoryCacheKeyStore.Add(cacheKey);
        return model;
    }

    public async Task<int> CreateAsync(UserRequestDTO userRequestDTO)
    {
        var user = new User
        {
            Username = userRequestDTO.Username,
            Fullname = userRequestDTO.Fullname
        };

        using var conn = context.CreateConnection();
        var sql = "INSERT INTO Users (Username, Fullname) VALUES (@Username, @Fullname); SELECT CAST(SCOPE_IDENTITY() as int);";
        return await conn.ExecuteScalarAsync<int>(sql, user);
    }
    public async Task<bool> UpdateAsync(int id, UserRequestDTO userRequestDTO)
    {
        using var conn = context.CreateConnection();
        var sql = "UPDATE Users SET Username = @Username, Fullname = @Fullname WHERE Id = @Id";
        var result = await conn.ExecuteAsync(sql, new
        {
            userRequestDTO.Username,
            userRequestDTO.Fullname,
            Id = id
        });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cacheKey = $"user-id-{id}";
        memoryCache.Remove(cacheKey);
        Console.WriteLine($"CACHE remove → {cacheKey}");

        using var conn = context.CreateConnection();
        var sql = "DELETE FROM Users WHERE Id = @id";
        var result = await conn.ExecuteAsync(sql, new { id });

        return result > 0;
    }
}