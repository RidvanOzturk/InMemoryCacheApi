using Dapper;
using DapperCacheExample.Data;
using InMemoryCacheExample.Data.Entities;
using InMemoryCacheExample.Data.Repositories.Contracts;

namespace InMemoryCacheExample.Data.Repositories.Implementations;

public class UserRepository(DapperContext context) : IUserRepository
{
    public async Task<User?> GetAsync(int id)
    {
        using var conn = context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
    }

    public async Task<int> CreateAsync(User user)
    {
        using var conn = context.CreateConnection();
        var sql = "INSERT INTO Users (Username, Fullname) VALUES (@Username, @Fullname); SELECT CAST(SCOPE_IDENTITY() as int);";
        return await conn.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<bool> UpdateAsync(int id, User user)
    {
        using var conn = context.CreateConnection();
        var sql = "UPDATE Users SET Username = @Username, Fullname = @Fullname WHERE Id = @Id";
        var result = await conn.ExecuteAsync(sql, new { user.Username, user.Fullname, Id = id });
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = context.CreateConnection();
        var sql = "DELETE FROM Users WHERE Id = @id";
        var result = await conn.ExecuteAsync(sql, new { id });
        return result > 0;
    }
}
