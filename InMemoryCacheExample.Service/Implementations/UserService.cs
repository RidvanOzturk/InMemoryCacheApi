using InMemoryCacheExample.Data.Entities;
using InMemoryCacheExample.Data.Repositories.Contracts;
using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheExample.Service.Implementations;

public class UserService(IUserRepository repository, IMemoryCache memoryCache) : IUserService
{
    private const string UserIdCacheKeyTemplate = "user-id-{0}";

    public async Task<UserResponseDTO?> GetAsync(int id)
    {
        var idKey = string.Format(UserIdCacheKeyTemplate, id);

        if (memoryCache.TryGetValue(idKey, out UserResponseDTO cached))
        {
            return cached;
        }

        var user = await repository.GetAsync(id);
        if (user is null) 
        {
            return null;
        }

        var dto = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Fullname = user.Fullname
        };
        memoryCache.Set(UserIdCacheKeyTemplate, dto, TimeSpan.FromMinutes(10));

        return dto;
    }

    public async Task<int> CreateAsync(UserRequestDTO userRequestDTO)
    {
        var user = new User
        {
            Username = userRequestDTO.Username,
            Fullname = userRequestDTO.Fullname
        };

        var response = new UserResponseDTO { Id = 0, Username = userRequestDTO.Username, Fullname = userRequestDTO.Fullname };
        var newId = await repository.CreateAsync(user);
        var idKey = string.Format(UserIdCacheKeyTemplate, newId);
        response.Id = newId;
        memoryCache.Set(idKey, response, TimeSpan.FromMinutes(10));

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
        if (!success)
        {
            return false;
        } 

        var updated = new UserResponseDTO
        {
            Id = id,
            Username = userRequestDTO.Username,
            Fullname = userRequestDTO.Fullname
        };

        var idKey = string.Format(UserIdCacheKeyTemplate, id);
        memoryCache.Set(idKey, updated, TimeSpan.FromMinutes(10));

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var idKey = string.Format(UserIdCacheKeyTemplate, id);
        memoryCache.Remove(idKey);
        var success = await repository.DeleteAsync(id);

        return success;
    }
}
