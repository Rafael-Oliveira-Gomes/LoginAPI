using Login.Domain.DTOs;
using Login.Domain.Entities;

namespace Login.Domain.Interfaces.Service;

public interface IUserService
{
    Task<List<ApplicationUser>> ListUsers();
    Task<ApplicationUser> GetUserById(string userId);
    Task<int> UpdateUser(ApplicationUser user);
    Task<bool> DeleteUser(string userId);
    Task<ApplicationUser> GetCurrentUser();
    Task<bool> SignUp(SignUpDTO signUpDTO);
    Task<SsoDTO> SignIn(SignInDTO signInDTO);
}
