using AuthenticationService.Api.FluentValidation;
using FluentValidation;
using FluentValidation.Results;

namespace AuthenticationService.Application.Services
{
    public class ValidationService<T>(IValidator<T> validator) : IValidationService<T> where T : class
    {
        private readonly IValidator<T> _validator = validator;

        public List<string> Validate(T response)
        {
            try
            {
                var result = _validator.Validate(response);
                if (!result.IsValid)
                {
                    List<string> errors = [];
                    foreach (ValidationFailure error in result.Errors)
                    {
                        errors.Add($"{error.PropertyName}:{error.ErrorMessage}");
                    }

                    return errors;
                }
                return [];
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
