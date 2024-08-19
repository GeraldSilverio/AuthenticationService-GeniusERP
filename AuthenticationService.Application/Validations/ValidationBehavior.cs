using AuthenticationService.Application.Response;
using FluentValidation;
using MediatR;

namespace AuthenticationService.Application.Validations
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Response<TResponse>>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<Response<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Response<TResponse>> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add($"{error.PropertyName}:{error.ErrorMessage}");
                }
                return new Response<TResponse>(errors, 400);
            }
            return await next();
        }
    }
}
