using AuthenticationService.Api.Features.Roles.Request;
using FluentValidation;

namespace AuthenticationService.Api.Features.Roles.Validations
{
    public class CreateRolCommandValidation : AbstractValidator<CreateRoleCommand>
    {
        public CreateRolCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
