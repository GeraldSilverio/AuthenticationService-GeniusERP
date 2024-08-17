using AuthenticationService.Application.Response;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Dtos.Account
{
    public class AuthUserDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Identification { get; set; }
        public string? Business { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string? BusinessId { get; set; }
        public string? CountryId { get; set; }
        public string? FirebaseId { get; set; }

        [JsonPropertyName("localId")]
        public string? LocalId { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("displayName")]
        public string? UserName { get; set; }

        [JsonPropertyName("idToken")]
        public string? IdToken { get; set; }
        [JsonPropertyName("error")]
        public Error? Error { get; set; }

        
    }
}
