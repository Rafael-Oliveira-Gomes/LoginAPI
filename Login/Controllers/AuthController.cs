using Login.Interface.Service;
using Login.Model.Dto;
using Login.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Login.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
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
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            try
            {
                bool isAuthenticated = await _authService.SignIn(signInDTO);
                return Ok(isAuthenticated);
            }
            catch (ArgumentException ex)
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
}
