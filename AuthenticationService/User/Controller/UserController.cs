using AuthenticationService.Api.User.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace AuthenticationService.Api.User.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            var response = await mediator.Send(registerUserCommand);
            return response.Errors != null ? BadRequest(response) : Ok(response);
        }
    }
}
