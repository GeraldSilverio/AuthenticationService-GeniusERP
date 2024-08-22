
using AuthenticationService.Api.Dto;
using AuthenticationService.Api.Dto.Account;
using AuthenticationService.Api.Interfaces;
using AuthenticationService.Api.Results;
using AuthenticationService.Api.Utilities;
using AuthenticationService.Application.Validations;
using FirebaseAdmin;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace AuthenticationService.Api.Features.User.Command
{
    public class LoginUserCommand : IRequest<Result<AuthUserDto>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginUserCommandHandler(IValidationService<LoginUserCommand> validator, IHttpClientFactory httpClientFactory, IConfiguration configuration) : IRequestHandler<LoginUserCommand, Result<AuthUserDto>>
    {
        private readonly IValidationService<LoginUserCommand> _validator = validator;
        public async Task<Result<AuthUserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<string> errors = _validator.Validate(request);
                if (errors.Count != 0) return new Result<AuthUserDto>(errors, 400);
                FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.Toke
                var result = await Login(request);

                return new Result<AuthUserDto>(result, 200);
            }
            catch (Exception ex)
            {
                return new Result<AuthUserDto>(ex);
            }
        }
        private async Task<AuthUserDto> Login(LoginUserCommand request)
        {
            try
            {
                StringContent content = PayloadExtensions.ToJsonPayload(new LoginDto(request.Email, request.Password,true));
                using HttpClient client = httpClientFactory.CreateClient("GoogleAuth");

                HttpResponseMessage response = await client.PostAsync($"{configuration["GoogleAPI:UrlAuth"]}{configuration["GoogleAPI:ApiKey"]}", content);

                AuthUserDto? authUser = await response.Content.ReadFromJsonAsync<AuthUserDto>();

                return authUser;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }


}
