using AuthenticationService.Application.Handler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Security
{
    public static class DependencyInjection
    {
        public static void AddSecurityLayer(this IServiceCollection services)
        {
            services.AddAuthentication("Firebase")
               .AddScheme<AuthenticationSchemeOptions, FireBaseHandler>("Firebase", null);
            services.AddAuthorization();
        }
    }
}
