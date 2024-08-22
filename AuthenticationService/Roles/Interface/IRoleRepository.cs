using AuthenticationService.Api.Roles;

namespace AuthenticationService.Api.Roles.Interface
{
    public interface IRoleRepository
    {
        Task<List<Rol>> GetAsync();
        Task<Rol> AddAsync(Rol rol);
    }
}
