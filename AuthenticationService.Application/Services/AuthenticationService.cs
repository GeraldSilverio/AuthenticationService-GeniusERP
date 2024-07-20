using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace AuthenticationService.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthUserDto> LoginAsync(LoginUserDto loginUserDto)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("GoogleAuth");
                var request = new { email = loginUserDto.Email, password = loginUserDto.Password, returnSecureToken = true };
                string user = JsonConvert.SerializeObject(request);
                HttpContent body = new StringContent(user, Encoding.UTF8, "application/json");

                HttpResponseMessage authResponse = await httpClient.PostAsync(httpClient.BaseAddress, body);

                AuthUserDto userResponse = await authResponse.Content.ReadFromJsonAsync<AuthUserDto>();
                return userResponse;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public Task<string> RegisterAsync(RegisterUserDto registerUser)
        {
            throw new NotImplementedException();
        }
    }
}
