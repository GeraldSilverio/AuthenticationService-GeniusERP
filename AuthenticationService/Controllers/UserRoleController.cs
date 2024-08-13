using AuthenticationService.Application.Dtos.UserRole;
using AuthenticationService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController(IUserRoleService userRoleService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRole createUserRole)
        {
            try
            {
                var result = await userRoleService.CreateUserRoleAsync(createUserRole);
                if (result) return Created("Se creo el rol para el usuario", createUserRole);
                return BadRequest($"No se pudo crear el rol para el usuario{createUserRole}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRole deleteUserRole)
        {
            var result = await userRoleService.DeleteUserRoleAsync(deleteUserRole);
            if (result) return NoContent();
            return BadRequest(deleteUserRole);
        }
    }
}
