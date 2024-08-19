using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Response;
using FluentValidation;

namespace AuthenticationService.Application.Services
{
    public class ValidationService<T>(IValidator<T> validator) : IValidationService<T> where T : class
    {
        private readonly IValidator<T> _validator = validator;

        public Response<T> Validate(T response)
        {
            try
            {
                var result = _validator.Validate(response);
                if (!result.IsValid)
                {
                    List<string> errors = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errors.Add($"{error.PropertyName}:{error.ErrorMessage}");
                    }

                    return new Response<T>(errors, 400);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
