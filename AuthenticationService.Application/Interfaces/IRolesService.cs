using AuthenticationService.Application.Dtos.Roles;
using AuthenticationService.Application.Response;

namespace AuthenticationService.Application.Interfaces
{
    public interface IRolesService
    {
        Task<Response<List<RolDto>>> GetRolesAsync();
        Task<Response<CreateRolDto>> AddRolAsync(CreateRolDto createRolDto);
    }
}
