using AuthenticationService.Api.Features.User.Command;
using AuthenticationService.Application.Dtos.Account;
using FluentValidation;

namespace AuthenticationService.Application.Validations
{
    public class LoginCommandValidation : AbstractValidator<LoginUserCommand>
    {
        public LoginCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Cannot be emptry")
                .NotNull().WithMessage("Cannot be empty")
                .EmailAddress().WithMessage("Incorrect formart");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Cannot be emptry")
                .NotNull().WithMessage("Cannot be empty");
        }
    }
}
