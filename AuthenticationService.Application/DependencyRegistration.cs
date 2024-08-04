using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Application
{
    public static class DependencyRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region ServicesDPI
            services.AddScoped<IAuthenticationService, Services.AuthenticationService>();
            services.AddScoped<IRolesService, RolesService>();
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

            #region Security
            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
                {
                    jwtOptions.Authority = configuration["GoogleAPI:ValidIssuer"];
                    jwtOptions.Audience = configuration["GoogleAPI:Audience"];
                    jwtOptions.TokenValidationParameters.ValidIssuer = configuration["GoogleAPI:ValidIssuer"];
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
