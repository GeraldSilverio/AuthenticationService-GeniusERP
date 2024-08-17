using AuthenticationService.Application.Handler;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IAuthenticationService = AuthenticationService.Application.Interfaces.IAuthenticationService;

namespace AuthenticationService.Application
{
    public static class DependencyRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region ServicesDPI
            services.AddScoped<IAuthenticationService, Services.AuthenticationService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IJwtService, JwtService>();
            #endregion

            #region HttpClientFactory

            services.AddHttpClient("GoogleAuth", (sp, httpClient) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                string? baseAddress = configuration["GoogleAPI:UrlAuth"];
                if (!string.IsNullOrEmpty(baseAddress))
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                }
            });

            #endregion

            #region FireBaseService
            services.AddSingleton(FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("FireBase.json")
            }));
            #endregion
        }
    }
}
