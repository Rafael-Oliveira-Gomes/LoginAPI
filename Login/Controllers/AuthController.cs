using Login.Domain.DTOs;
using Login.Domain.Entities;
using Login.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers;

/// <summary>
/// Controlador responsável pelas operações de autenticação e autorização de usuários.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _authService;

    /// <summary>
    /// Construtor do controlador de autenticação.
    /// </summary>
    /// <param name="authService">Serviço de autenticação de usuários.</param>
    public AuthController(IUserService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Realiza o cadastro de um novo usuário.
    /// </summary>
    /// <param name="signUpDTO">Dados do usuário para cadastro.</param>
    /// <returns>Retorna sucesso ou mensagem de erro.</returns>
    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
    {
        bool ret = await _authService.SignUp(signUpDTO);

        return Ok(ret);
    }

    /// <summary>
    /// Realiza o login do usuário e retorna o token de autenticação.
    /// </summary>
    /// <param name="signInDTO">Dados de login do usuário.</param>
    /// <returns>Retorna o token de autenticação ou mensagem de erro.</returns>
    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        SsoDTO ssoDTO = await _authService.SignIn(signInDTO);

        return Ok(ssoDTO);
    }

    /// <summary>
    /// Obtém os dados do usuário atualmente autenticado.
    /// </summary>
    /// <returns>Retorna os dados do usuário ou mensagem de erro.</returns>
    [HttpGet("get-current-user")]
    public async Task<ActionResult> GetCurrentUser()
    {
        ApplicationUser currentUser = await _authService.GetCurrentUser();

        return Ok(currentUser);
    }
}