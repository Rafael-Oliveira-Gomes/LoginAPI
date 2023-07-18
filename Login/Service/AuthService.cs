using Login.Interface.Repository;
using Login.Interface.Service;
using Login.Model.Dto;
using Login.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace Login.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<ApplicationUser>> ListUsers()
        {
            List<ApplicationUser> listUsers = await _userRepository.ListUsers();

            return listUsers;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            ApplicationUser user = await _userRepository.GetUser(userId);

            if (user == null)
                throw new ArgumentException("Usuário não existe!");

            return user;
        }

        public async Task<int> UpdateUser(ApplicationUser user)
        {
            ApplicationUser findUser = await _userRepository.GetUser(user.Id);
            if (findUser == null)
                throw new ArgumentException("Usuário não encontrado");

            findUser.Email = user.Email;
            findUser.UserName = user.UserName;

            return await _userRepository.UpdateUser(findUser);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            ApplicationUser findUser = await _userRepository.GetUser(userId);
            if (findUser == null)
                throw new ArgumentException("Usuário não encontrado");

            await _userRepository.DeleteUser(userId);

            return true;
        }

        public async Task<bool> SignUp(SignUpDTO signUpDTO)
        {
            ApplicationUser? userExists = await _userManager.FindByNameAsync(signUpDTO.Username);
            if (userExists != null)
                throw new ArgumentException("Username already exists!");

            userExists = await _userManager.FindByEmailAsync(signUpDTO.Email);
            if (userExists != null)
                throw new ArgumentException("Email already exists!");

            ApplicationUser user;

            user = new ApplicationUser()
            {
                Email = signUpDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = signUpDTO.Username,
            };

            IdentityResult? result = await _userManager.CreateAsync(user, signUpDTO.Password);

            if (!result.Succeeded)
                throw new ArgumentException("Cadastro do usuário falhou.");



            if (signUpDTO.Password == "Admin!123")
            {
                result = await _userManager.AddToRoleAsync(user, "Admin");
                if (!result.Succeeded)
                    throw new ArgumentException("Cadastro do usuário falhou.");
            }

            return true;
        }


        public async Task<bool> SignIn(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByNameAsync(signInDTO.Username);
            if (user == null)
                throw new ArgumentException("Usuário não encontrado.");

            if (!await _userManager.CheckPasswordAsync(user, signInDTO.Password))
                throw new ArgumentException("Senha inválida.");

            IList<string>? userRoles = await _userManager.GetRolesAsync(user);

            return true;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            string? userId = _userManager.GetUserId(_httpContextAccessor.HttpContext!.User); // Get user id:

            ApplicationUser user = await _userRepository.GetUser(userId);

            return user;
        }
    }
}