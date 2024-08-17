using AuthenticationService.Application.Dtos.Account;

namespace AuthenticationService.Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GetCustomTokenAsync(AuthUserDto user, string userId);
    }
}
