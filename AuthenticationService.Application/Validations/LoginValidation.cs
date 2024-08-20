//using AuthenticationService.Application.Dtos.Account;
//using FluentValidation;

//namespace AuthenticationService.Application.Validations
//{
//    public class LoginValidation : AbstractValidator<lOGINuSER>
//    {
//        public LoginValidation()
//        {
//            RuleFor(x => x.Email).NotEmpty().WithMessage("Cannot be emptry")
//                .NotNull().WithMessage("Cannot be empty")
//                .EmailAddress().WithMessage("Incorrect formart");

//            RuleFor(x => x.Password).NotEmpty().WithMessage("Cannot be emptry")
//                .NotNull().WithMessage("Cannot be empty");
//        }
//    }
//}
