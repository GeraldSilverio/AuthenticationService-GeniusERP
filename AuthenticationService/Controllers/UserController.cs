using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Response;
using FluentValidation;
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
            Response<AuthUserDto> response = await authenticationService.LoginAsync(loginUserDto);

            return response.Errors != null ? BadRequest(response) : Created("Login existoso",response);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var response = await authenticationService.RegisterAsync(registerUserDto);
            return response.Code != 201 ? BadRequest(response) : Created("Usuario creado",response);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            bool response = await authenticationService.ResetPassword(resetPasswordDto);
            return response ? Ok() : BadRequest();
        }
    }
}
