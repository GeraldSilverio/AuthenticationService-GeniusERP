using AuthenticationService.Api.Roles.Request;
using FluentValidation;

namespace AuthenticationService.Api.Roles.Validations
{
    public class CreateRolCommandValidation : AbstractValidator<CreateRoleCommand>
    {
        public CreateRolCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
