using AuthenticationService.Application.Dtos.Account;

namespace AuthenticationService.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(RegisterUserDto user,string fireBaseCode);
        //Task<User> GetUserAsync(string email);
    }
}
