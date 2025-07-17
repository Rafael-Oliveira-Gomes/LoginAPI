using Login.Domain.DTOs;
using Login.Domain.Entities;
using Login.Domain.Interfaces.Repositories;
using Login.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Login.Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserService(
        IUserRepository userRepository,
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
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
        var userExists = await _userManager.FindByNameAsync(signUpDTO.Username);
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
            UserName = signUpDTO.Username
        };

        var result = await _userManager.CreateAsync(user, signUpDTO.Password);

        if (!result.Succeeded)
            throw new ArgumentException("Cadastro do usuário falhou.");

        return true;
    }

    public async Task<SsoDTO> SignIn(SignInDTO signInDTO)
    {
        var user = await _userManager.FindByEmailAsync(signInDTO.Email);
        if (user == null)
            throw new ArgumentException("Usuário não encontrado.");

        if (!await _userManager.CheckPasswordAsync(user, signInDTO.Password))
            throw new ArgumentException("Senha inválida.");

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return new SsoDTO(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }

    public async Task<ApplicationUser> GetCurrentUser()
    {
        var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User); // Get user id:

        ApplicationUser user = await _userRepository.GetUser(userId!);

        return user;
    }
}