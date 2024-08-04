using AuthenticationService.Application.Dtos.Roles;
using AuthenticationService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRolesService rolesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var response = await rolesService.GetRolesAsync();
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRol([FromBody] CreateRolDto createRolDto)
        {
            await rolesService.AddRolAsync(createRolDto);
            return Created();
        }
    }
}
