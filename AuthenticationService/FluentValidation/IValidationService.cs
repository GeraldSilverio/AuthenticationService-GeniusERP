using AuthenticationService.Application.Response;

namespace AuthenticationService.Api.FluentValidation
{
    public interface IValidationService<T> where T : class
    {
        List<string> Validate(T response);
    }
}
