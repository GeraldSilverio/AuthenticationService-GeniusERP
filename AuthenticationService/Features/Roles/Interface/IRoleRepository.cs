using AuthenticationService.Api.Entities;

namespace AuthenticationService.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Rol>> GetAsync();
        Task<Rol> AddAsync(Rol rol);
    }
}
