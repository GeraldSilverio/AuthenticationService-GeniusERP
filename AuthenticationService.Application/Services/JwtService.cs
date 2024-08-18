using AuthenticationService.Application.Dtos.Account;
using AuthenticationService.Application.Dtos.UserRole;
using AuthenticationService.Application.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using AuthenticationService.Application.Dtos.Account;

namespace AuthenticationService.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IConfiguration _configuration;

        public JwtService(IUserRoleService userRoleService, IConfiguration configuration)
        {
            _userRoleService = userRoleService;
            _configuration = configuration;
        }

        private async Task<string> AuthenticateWithCustomTokenAsync(string customToken)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postData = new
                    {
                        token = customToken,
                        returnSecureToken = true
                    };

                    var content = new StringContent(JsonSerializer.Serialize(postData), System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync($"{_configuration["GoogleAPI:CustomTokenUrl"]}{_configuration["GoogleAPI:ApiKey"]}", content);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<SingInResponse>(responseBody);

                    return result.IdToken;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
        public async Task<string> GetCustomTokenAsync(AuthUserDto AuthUserDto,string userId)
        {
            try
            {
                List<GetUserRole> roles = await _userRoleService.GetUserRolesAsync(userId);
                if (roles.Count > 0)
                {
                    List<string> roleList = roles.Select(role => role.RolId).ToList();

                    Dictionary<string, object> claims = new()
                     {
                            {"email",AuthUserDto.Email},
                            { "roles", roleList }
                     };

                    string token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userId, claims);
                    string customToken = await AuthenticateWithCustomTokenAsync(token);
                    return customToken;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new ApplicationException($"Error al generar el token personalizado: {ex.Message}", ex);
            }
        }
    }
}
