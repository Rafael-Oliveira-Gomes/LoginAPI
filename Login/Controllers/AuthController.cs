using Login.Domain.DTOs;
using Login.Domain.Entities;
using Login.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _authService;

    public AuthController(IUserService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
    {
        try
        {
            bool ret = await _authService.SignUp(signUpDTO);

            return Ok(ret);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        try
        {
            SsoDTO ssoDTO = await _authService.SignIn(signInDTO);

            return Ok(ssoDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-current-user")]
    public async Task<ActionResult> GetCurrentUser()
    {
        try
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            return Ok(currentUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
