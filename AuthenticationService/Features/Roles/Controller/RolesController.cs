using AuthenticationService.Api.Features.Roles.Request;
using AuthenticationService.Application.Dtos.Roles;
using AuthenticationService.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Api.Features.Roles.Controller
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
