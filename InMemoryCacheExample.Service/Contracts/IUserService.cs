namespace InMemoryCacheExample.Service.Contracts;

public interface IUserService
{
    Task<int> GetUserCountAsync();
}
