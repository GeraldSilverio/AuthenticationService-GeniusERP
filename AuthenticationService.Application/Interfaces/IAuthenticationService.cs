using AuthenticationService.Application.Dtos.Account;

namespace AuthenticationService.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthUserDto> LoginAsync(LoginUserDto loginUserDto);
        Task<string> RegisterAsync(RegisterUserDto registerUserDto);
        Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
