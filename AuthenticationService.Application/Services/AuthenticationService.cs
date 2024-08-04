using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text;

namespace AuthenticationService.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<AuthUserDto> LoginAsync(LoginUserDto loginUserDto)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("GoogleAuth");

                var payload = new { email = loginUserDto.Email, password = loginUserDto.Password, returnSecureToken = true };

                string user = JsonConvert.SerializeObject(payload);

                HttpContent body = new StringContent(user, Encoding.UTF8, "application/json");

                HttpResponseMessage authResponse = await httpClient.PostAsync($"{httpClient.BaseAddress}{_configuration["GoogleAPI:ApiKey"]}", body);

                AuthUserDto? userResponse = await authResponse.Content.ReadFromJsonAsync<AuthUserDto>();

                if (authResponse.IsSuccessStatusCode && userResponse.Error == null)
                {
                    bool isEmailConfirm = await IsEmailConfirm(userResponse.LocalId);

                    if (!isEmailConfirm) userResponse.Error = new Response.Error
                    {
                        Code = 401,
                        Errors = null,
                        Message = "Tu email no esta confirmado, por favor hazlo para poder usar la aplicacion."
                    };
                }
                return userResponse;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<string> RegisterAsync(RegisterUserDto registerUserDto)
        {
            try
            {
                UserRecordArgs userRecord = new()
                {
                    Disabled = false,
                    DisplayName = registerUserDto.UserName,
                    Email = registerUserDto.Email,
                    EmailVerified = false,
                    Password = registerUserDto.Password,
                    PhoneNumber = "+" + registerUserDto.PhoneNumber,
                };
                UserRecord userCreated = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecord);
                if (userCreated != null)
                {
                    AuthUserDto token = await LoginAsync(new LoginUserDto { Email = userCreated.Email, Password = registerUserDto.Password });
                    _ = Task.Run(async () => await SendEmailConfirmation(token.IdToken));
                }
                return userCreated.Uid;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task SendEmailConfirmation(string token)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("GoogleAuth");
                var payload = new
                {
                    requestType = "VERIFY_EMAIL",
                    idToken = token,
                };
                string request = JsonConvert.SerializeObject(payload);
                HttpContent content = new StringContent(request, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_configuration["GoogleAPI:EmailVerificationUrl"]}{_configuration["GoogleAPI:ApiKey"]}", content);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);

            }
        }

        private async Task<bool> IsEmailConfirm(string idToken)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(idToken);
                if (userRecord != null)
                {
                    return userRecord.EmailVerified ? true : false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public async Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("GoogleAuth");
                var payload = new
                {
                    requestType = "PASSWORD_RESET",
                    email = resetPasswordDto.Email,
                };
                string request = JsonConvert.SerializeObject(payload);
                HttpContent content = new StringContent(request, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_configuration["GoogleAPI:EmailVerificationUrl"]}{_configuration["GoogleAPI:ApiKey"]}", content);

                return response.IsSuccessStatusCode ? true : false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
