using InMemoryCacheExample.Data.Entities;
using InMemoryCacheExample.Data.Repositories.Contracts;
using InMemoryCacheExample.Service.Caching;
using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheExample.Service.Implementations;

public class UserService(IUserRepository repository, IMemoryCache memoryCache) : IUserService
{
    public async Task<UserResponseDTO?> GetAsync(int id)
    {
        var cacheKey = $"user-id-{id}";

        if (memoryCache.TryGetValue(cacheKey, out UserResponseDTO cached))
        {
            Console.WriteLine("There is cache");
            return cached;
        }

        Console.WriteLine("There is no cache, going to db");

        var user = await repository.GetAsync(id);
        if (user is null) return null;

        var dto = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Fullname = user.Fullname
        };

        memoryCache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        MemoryCacheKeyStore.Add(cacheKey);

        return dto;
    }

    public async Task<int> CreateAsync(UserRequestDTO userRequestDTO)
    {
        var user = new User
        {
            Username = userRequestDTO.Username,
            Fullname = userRequestDTO.Fullname
        };

        var usernameKey = $"user-username-{userRequestDTO.Username}";
        var response = new UserResponseDTO { Id = 0, Username = userRequestDTO.Username, Fullname = userRequestDTO.Fullname };
        memoryCache.Set(usernameKey, response, TimeSpan.FromMinutes(10));
        MemoryCacheKeyStore.Add(usernameKey);

        var newId = await repository.CreateAsync(user);

        var idKey = $"user-id-{newId}";
        response.Id = newId;
        memoryCache.Set(idKey, response, TimeSpan.FromMinutes(10));
        MemoryCacheKeyStore.Add(idKey);

        return newId;
    }

    public async Task<bool> UpdateAsync(int id, UserRequestDTO userRequestDTO)
    {
        var user = new User
        {
            Username = userRequestDTO.Username,
            Fullname = userRequestDTO.Fullname
        };

        var success = await repository.UpdateAsync(id, user);
        if (!success) return false;

        var updated = new UserResponseDTO
        {
            Id = id,
            Username = userRequestDTO.Username,
            Fullname = userRequestDTO.Fullname
        };

        var idKey = $"user-id-{id}";
        var usernameKey = $"user-username-{userRequestDTO.Username}";

        memoryCache.Set(idKey, updated, TimeSpan.FromMinutes(10));
        memoryCache.Set(usernameKey, updated, TimeSpan.FromMinutes(10));
        MemoryCacheKeyStore.Add(idKey);
        MemoryCacheKeyStore.Add(usernameKey);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var idKey = $"user-id-{id}";
        memoryCache.Remove(idKey);
        MemoryCacheKeyStore.Remove(idKey);

        var success = await repository.DeleteAsync(id);
        return success;
    }
}
