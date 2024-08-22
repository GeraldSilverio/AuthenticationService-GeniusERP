namespace AuthenticationService.Api.UserRole.Domain
{
    public class CreateUserRole
    {
        public string? UserId { get; set; }
        public string RoleId { get; set; }
        public string? CreatedBy { get; set; }
    }
}
