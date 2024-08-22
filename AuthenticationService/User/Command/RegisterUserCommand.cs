using AuthenticationService.Api.Results;
using AuthenticationService.Api.User.Repositories;
using AuthenticationService.Api.UserRole.Domain;
using AuthenticationService.Api.UserRole.Repository;
using FirebaseAdmin.Auth;
using MediatR;

namespace AuthenticationService.Api.User.Command
{
    public class RegisterUserCommand : IRequest<Result<Unit>>
    {
        public string Names { get; set; } = null!;
        public string LastNames { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public char Gender { get; set; }
        public string Address { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public int CountryId { get; set; }
        public int TypeIdentificationId { get; set; }
        public string BusinessId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public string CreateBy { get; set; } = null!;
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Unit>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRoleRepository userRoleRepository, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<Unit>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UserRecordArgs userRecord = new()
                {
                    DisplayName = request.UserName,
                    Email = request.Email,
                    Disabled = false,
                    EmailVerified = false,
                    Password = request.Password,
                    PhoneNumber = request.PhoneNumber,
                };
                //Creando el usuario en FireBase.
                UserRecord userCreated = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecord, CancellationToken.None);

                //Creando el usuario y asignando los roles.
                string codeUser = await _userRepository.CreateUserAsync(request, userCreated.Uid);
                await SetRoles(codeUser, request.RoleId);

                //Buscando el rol para asignarlo en el token.
                List<GetUserRole> userRole = await _userRoleRepository.GetUserRolesAsync(codeUser);

                //Agregar los claims personalizados al usuario.
                Dictionary<string, object> claims = [];
                foreach (GetUserRole role in userRole)
                {
                    claims.Add("rol", role.Name);
                }
                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userCreated.Uid, claims, CancellationToken.None);

                return new Result<Unit>(Unit.Value, 200);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SetRoles(string userId, string roleId)
        {
            try
            {
                CreateUserRole createUserRole = new() { CreatedBy = "Genius_Web", RoleId = roleId, UserId = userId };
                await _userRoleRepository.CreateUserRoleAsync(createUserRole);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
