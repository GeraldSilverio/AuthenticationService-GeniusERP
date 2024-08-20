using System.Text;
using System.Text.Json;

namespace AuthenticationService.Api.Utilities
{
    public static class PayloadExtensions
    {
        public static StringContent ToJsonPayload<T>(this T payload)
        {
            var stringPayload = JsonSerializer.Serialize(payload);
            return new StringContent(stringPayload, Encoding.UTF8, "application/json");
        }
    }
}
