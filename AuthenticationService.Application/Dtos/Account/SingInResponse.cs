using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Dtos.Account
{
    public record SingInResponse
    {
        [JsonPropertyName("idToken")]
        public string? IdToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("expiresIn")]
        public string? ExpiresIn { get; set; }
        [JsonPropertyName("localId")]
        public string? LocalId { get; set; }
    }
}
