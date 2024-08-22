using AuthenticationService.Api.User.Command;

namespace AuthenticationService.Api.User.Repositories
{
    public interface IUserRepository
    {
        Task<string> CreateUserAsync(RegisterUserCommand user, string fireBaseCode);
        //Task<User> GetUserAsync(string email);
    }
}
