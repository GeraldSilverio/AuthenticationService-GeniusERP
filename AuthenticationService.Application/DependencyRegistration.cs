//using AuthenticationService.Application.Dtos.Account;
//using AuthenticationService.Application.Interfaces;
//using AuthenticationService.Application.Services;
//using AuthenticationService.Application.Validations;
//using FirebaseAdmin;
//using Google.Apis.Auth.OAuth2;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace AuthenticationService.Application
//{
//    public static class DependencyRegistration
//    {
//        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
//        {
//            #region ServicesDPI
//            services.AddScoped<IFireBaseService, FireBaseService>();
//            services.AddScoped<IUserService, UserService>();
//            services.AddScoped<IRolesService, RolesService>();
//            services.AddScoped<IUserRoleService, UserRoleService>();
//            services.AddScoped<IJwtService, JwtService>();
//            services.AddScoped(typeof(IValidationService<>), typeof(ValidationService<>));
//            services.AddSingleton<IValidator<LoginUserDto>, LoginValidation>();
//            #endregion

//            #region HttpClientFactory

//            services.AddHttpClient("GoogleAuth", (sp, httpClient) =>
//            {
//                var configuration = sp.GetRequiredService<IConfiguration>();

//                string? baseAddress = configuration["GoogleAPI:UrlAuth"];
//                if (!string.IsNullOrEmpty(baseAddress))
//                {
//                    httpClient.BaseAddress = new Uri(baseAddress);
//                }
//            });

//            #endregion

//            #region FireBaseService
//            services.AddSingleton(FirebaseApp.Create(new AppOptions
//            {
//                Credential = GoogleCredential.FromFile("FireBase.json")
//            }));
//            #endregion
//        }
//    }
//}
