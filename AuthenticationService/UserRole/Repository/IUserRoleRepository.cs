using AuthenticationService.Api.UserRole.Domain;

namespace AuthenticationService.Api.UserRole.Repository
{
    public interface IUserRoleRepository
    {
        Task<bool> CreateUserRoleAsync(CreateUserRole createUserRole);
        Task<List<GetUserRole>> GetUserRolesAsync(string userId);

        Task<bool> DeleteUserRoleAsync(DeleteUserRole deleteUserRole);
    }
}
