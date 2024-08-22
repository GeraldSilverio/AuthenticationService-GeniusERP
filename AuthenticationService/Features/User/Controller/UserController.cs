using AuthenticationService.Api.Features.User.Command;
using AuthenticationService.Api.Results;
using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace AuthenticationService.Api.Features.User.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Produces<LoginUserCommand>]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            var response = await mediator.Send(loginUserCommand);

            return response.Errors != null ? BadRequest(response) : Created("Login existoso", response);
        }
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        //{
        //    var response = await fireBaseService.RegisterAsync(registerUserDto);
        //    return response.Code != 201 ? BadRequest(response) : Created("Usuario creado", response);
        //}
        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        //{
        //    bool response = await fireBaseService.ResetPassword(resetPasswordDto);
        //    return response ? Ok() : BadRequest();
        //}
    }
}
