using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IAuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            AuthUserDto response = await authenticationService.LoginAsync(loginUserDto);

            return response.Error != null ? BadRequest(response.Error) : Ok(response);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            string response = await authenticationService.RegisterAsync(registerUserDto);
            return string.IsNullOrEmpty(response) ? BadRequest() : Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            bool response = await authenticationService.ResetPassword(resetPasswordDto);
            return response ? Ok() : BadRequest();
        }
    }
}
