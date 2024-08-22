using AuthenticationService.Api.Roles.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Api.Roles.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IMediator mediator) : ControllerBase
    {
        //[Authorize(AuthenticationSchemes = "Firebase", Roles = "Vendedor")]
        //[HttpGet]
        //public async Task<IActionResult> GetRoles()
        //{
        //    var response = await rolesService();
        //    return Ok(response);
        //}

        [HttpPost]
        public async Task<IActionResult> AddRol([FromBody] CreateRoleCommand createRoleCommand)
        {
            var result = await mediator.Send(createRoleCommand);

            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
