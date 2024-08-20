using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AuthenticationService.Application.Handler
{
    public class AuthorizationMiddleware : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// Constructor por defecto de este Handler. 
        //Hereda del base de la clase AuthenticationSchemeOptions.
        public AuthorizationMiddleware(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                //Validando que en los hearders viaje el Authrization, que es donde se envia el JWT.
                if (!Request.Headers.ContainsKey("Authorization"))
                    return AuthenticateResult.Fail("Authorization header is missing");

                //Extrañendo el JWT del header.
                string? token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrWhiteSpace(token)) return AuthenticateResult.Fail("JWT is missing");
                
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

                // Crear la identidad del usuario
                var identity = new ClaimsIdentity(CreateClaims(decodedToken), nameof(AuthorizationMiddleware));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);

                // Devolver un resultado de autenticación exitoso
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                // Devolver un resultado de autenticación fallido
                return AuthenticateResult.Fail($"Invalid Firebase token: {ex.Message}");
            }
        }
        private List<Claim> CreateClaims(FirebaseToken firebaseToken)
        {
            bool rolesClaims = firebaseToken.Claims.TryGetValue("roles", out var rolesObject);
            bool emailClaim = firebaseToken.Claims.TryGetValue("email", out var emailObject);

            // Convertir el claim a una cadena
            string? rolesJson = rolesObject.ToString();
            string? email = emailObject.ToString();

            // Deserializar la cadena JSON en una lista de cadenas
            List<string> roles = JsonSerializer.Deserialize<List<string>>(rolesJson);

            // Crear los claims con base en la información decodificada
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, firebaseToken.Uid),
                new Claim(ClaimTypes.Email, email),
            };
            //Por cada role, crear un claim.
            foreach (var role in roles)
            {
                var rolClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(rolClaim);
            }
            return claims;
        }
    }
}
