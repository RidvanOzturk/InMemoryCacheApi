using InMemoryCacheExample.Service.DTOs;

namespace InMemoryCacheExample.Service.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetAllAsync();
    Task<UserResponseDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(UserRequestDTO userRequestDTO);
    Task<bool> UpdateAsync(int id, UserRequestDTO userRequestDTO);
    Task<bool> DeleteAsync(int id);

}
