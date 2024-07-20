using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Response
{
    public class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("errors")]
        public ErrorDetails[]? Errors { get; set; }

    }
    public class ErrorDetails
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("domain")]
        public string? Domain { get; set; }
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}
