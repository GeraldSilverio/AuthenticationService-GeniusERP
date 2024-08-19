using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Dtos.UserRole;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Response;
using AuthenticationService.Domain.Models;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace AuthenticationService.Application.Services
{
    public class FireBaseService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUserService userService, IUserRoleService userRoleService, IJwtService jwtService,IValidationService<LoginUserDto> loginValidation ) : IFireBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserService _userService = userService;
        private readonly IUserRoleService _userRoleService = userRoleService;
        private readonly IJwtService _jwtService = jwtService;

        #region LoginUser
        public async Task<Response<AuthUserDto>> LoginAsync(LoginUserDto loginUserDto)
        {
            try
            {
                // Verificar si el usuario existe
                User user = await _userService.GetUserAsync(loginUserDto.Email);
                if (user == null)
                {
                    return new Response<AuthUserDto>(new List<string> { "Usuario/Contraseña inválidos" }, 400);
                }

                AuthUserDto userResponse = await SendLoginRequest(loginUserDto.Email, loginUserDto.Password);

                if (userResponse != null)
                {
                    bool isEmailConfirm = await IsEmailConfirm(userResponse.LocalId);

                    if (!isEmailConfirm)
                    {
                        return new Response<AuthUserDto>(new List<string> { "El correo electrónico no ha sido confirmado" }, 400);
                    }

                    #region Asignando al usuario sus valores de base de datos
                    userResponse.Business = user.Business;
                    userResponse.BusinessId = user.BusinessId;
                    userResponse.Address = user.Address;
                    userResponse.Country = user.Country;
                    userResponse.CountryId = user.CountryId;
                    userResponse.Identification = user.Identification;
                    userResponse.Name = user.Name;
                    userResponse.LastName = user.LastName;
                    userResponse.IdToken = await _jwtService.GetCustomTokenAsync(userResponse, user.UserId);
                    #endregion

                    return new Response<AuthUserDto>(userResponse, 201);
                }
                return new Response<AuthUserDto>(new List<string> { "Ocurrió un problema al autenticarse, favor revisar los logs." }, 400);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return new Response<AuthUserDto>(new List<string> { $"Error interno: {ex.Message}" }, 500);
            }
        }

        private async Task<AuthUserDto> SendLoginRequest(string email, string password)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("GoogleAuth");

            // Preparar el payload para la solicitud de autenticación
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };
            string request = JsonConvert.SerializeObject(payload);
            HttpContent body = new StringContent(request, Encoding.UTF8, "application/json");
            // Realizar la solicitud de autenticación
            HttpResponseMessage authResponse = await httpClient.PostAsync($"{httpClient.BaseAddress}{_configuration["GoogleAPI:ApiKey"]}", body);

            // Leer la respuesta
            AuthUserDto? userResponse = await authResponse.Content.ReadFromJsonAsync<AuthUserDto>();

            if (authResponse.IsSuccessStatusCode && userResponse?.Error == null)
            {
                return userResponse;
            }

            return null;

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
                    return new Response<string>("Usuario creado con exito", 201);
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
            AuthUserDto login = await SendLoginRequest(user.Email, registerUserDto.Password);
            _ = Task.Run(async () => await SendEmailConfirmation(login.IdToken));
            string userId = await _userService.CreateUserAsync(registerUserDto, user.Uid);

            //Agregar los roles de ese usuario.
            CreateUserRole userRole = new()
            {
                RolesId = registerUserDto.RolesId,
                UserId = userId,
                CreatedBy = registerUserDto.CreateBy,
            };

            await _userRoleService.CreateUserRoleAsync(userRole);
        }
        #endregion

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
