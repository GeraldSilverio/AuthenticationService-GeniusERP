using AuthenticationService.Application.Dtos.Account;

namespace AuthenticationService.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterUserDto registerUser);
        Task<AuthUserDto> LoginAsync(LoginUserDto loginUserDto);
    }
}
