using System.Text.Json.Serialization;

namespace AuthenticationService.Api.Dto
{
    public class LoginDto
    {
        public LoginDto(string email, string password,bool returnSecureToken)
        {
            Email = email;
            Password = password;
            ReturnSecureToken = returnSecureToken;
        }

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
        
        [JsonPropertyName("password")]
        public string Password {  get; set; }

        [JsonPropertyName("returnSecureToken")]
        private bool ReturnSecureToken {  get; set; }

        
    }
}
