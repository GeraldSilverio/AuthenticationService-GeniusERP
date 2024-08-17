using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Handler
{
    public static class GeraldTest
    {
        public static async Task<string> AuthenticateWithCustomTokenAsync(string customToken)
        {
            using (HttpClient client = new HttpClient())
            {
                var postData = new
                {
                    token = customToken,
                    returnSecureToken = true
                };

                var content = new StringContent(JsonSerializer.Serialize(postData), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key=AIzaSyAjOCy9BuwygJsFHxKpAZpDW5BBCsRoNiE", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SignInResponse>(responseBody);

                return result.IdToken; // Retorna el ID Token obtenido
            }
        }
    }
}
public class SignInResponse
{
    [JsonPropertyName("idToken")]
    public string IdToken { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
    [JsonPropertyName("expiresIn")]
    public string ExpiresIn { get; set; }
    [JsonPropertyName("localId")]
    public string LocalId { get; set; }
}
