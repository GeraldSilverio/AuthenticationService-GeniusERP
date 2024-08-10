using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Response;

namespace AuthenticationService.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthUserDto> LoginAsync(LoginUserDto loginUserDto);
        Task<Response<string>> RegisterAsync(RegisterUserDto registerUserDto);
        Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
