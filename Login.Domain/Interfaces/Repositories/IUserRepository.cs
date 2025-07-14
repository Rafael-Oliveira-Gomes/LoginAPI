using Login.Domain.Entities;

namespace Login.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<ApplicationUser>> ListUsers();
    Task<ApplicationUser> GetUser(string userId);
    Task<ApplicationUser> CreateUser(ApplicationUser user);
    Task<int> UpdateUser(ApplicationUser user);
    Task<bool> DeleteUser(string userId);
}
