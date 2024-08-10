using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Dtos.UserRole;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Response;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace AuthenticationService.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public AuthenticationService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUserService userService, IUserRoleService userRoleService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _userService = userService;
            _userRoleService = userRoleService;
        }

        #region LoginUser
        public async Task<AuthUserDto> LoginAsync(LoginUserDto loginUserDto)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("GoogleAuth");

                var payload = new
                {
                    email = loginUserDto.Email,
                    password = loginUserDto.Password,
                    returnSecureToken = true
                };

                string user = JsonConvert.SerializeObject(payload);

                HttpContent body = new StringContent(user, Encoding.UTF8, "application/json");

                HttpResponseMessage authResponse = await httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_configuration["GoogleAPI:ApiKey"]}", body);

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

        #endregion

        #region RegisterByWeb
        public async Task<Response<string>> RegisterAsync(RegisterUserDto registerUserDto)
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
                    PhoneNumber = "+1" + registerUserDto.PhoneNumber,
                };
                UserRecord userCreated = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecord);
                if (userCreated != null)
                {
                    await ExecuteAfterRegister(userCreated, registerUserDto);
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task ExecuteAfterRegister(UserRecord user, RegisterUserDto registerUserDto)
        {
            AuthUserDto token = await LoginAsync(new LoginUserDto { Email = user.Email, Password = registerUserDto.Password });
            _ = Task.Run(async () => await SendEmailConfirmation(token.IdToken));
            string userId = await _userService.CreateUserAsync(registerUserDto, user.Uid);

            //Agregar los roles de ese usuario.
            CreateUserRole userRole = new()
            {
                RolesId = new List<string> { registerUserDto.RolId },
                UserId = userId
            };

            await _userRoleService.CreateUserRoleAsync(userRole);
            #endregion

        }

        #region SendEmailConfirmation
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

        #endregion

        #region ResetPassword
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
        #endregion
    }
}
