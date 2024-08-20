using AuthenticationService.Application.Response;

namespace AuthenticationService.Api.Interfaces
{
    public interface IValidationService<T> where T : class
    {
        List<string> Validate(T response);
    }
}
