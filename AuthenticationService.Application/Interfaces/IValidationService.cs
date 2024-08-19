using AuthenticationService.Application.Response;

namespace AuthenticationService.Application.Interfaces
{
    public interface IValidationService<T> where T : class
    {
        Response<T> Validate(T response);
    }
}
