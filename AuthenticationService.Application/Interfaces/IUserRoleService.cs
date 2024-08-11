using AuthenticationService.Application.Dtos.UserRole;

namespace AuthenticationService.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<bool> CreateUserRoleAsync(CreateUserRole createUserRole);
        Task<List<GetUserRole>> GetUserRolesAsync(string userId);
    }
}
