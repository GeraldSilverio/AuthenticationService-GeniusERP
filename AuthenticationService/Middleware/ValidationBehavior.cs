using AuthenticationService.Api.Results;
using FluentValidation;
using MediatR;

namespace AuthenticationService.Application.Validations
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, Result<TResponse>>
        where TRequest : IRequest<TResponse>
        where TResponse : Result<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add($"{error.PropertyName}:{error.ErrorMessage}");
                }
                return new Result<TResponse>(errors, 400);
            }
            return await next();
        }
    }
}
