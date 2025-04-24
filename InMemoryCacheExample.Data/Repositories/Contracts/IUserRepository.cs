using InMemoryCacheExample.Data.Entities;

namespace InMemoryCacheExample.Data.Repositories.Contracts;

public interface IUserRepository
{
    Task<User?> GetAsync(int id);
    Task<int> CreateAsync(User user);
    Task<bool> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
}
