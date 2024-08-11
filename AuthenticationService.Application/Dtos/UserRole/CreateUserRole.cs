namespace AuthenticationService.Application.Dtos.UserRole
{
    public class CreateUserRole
    {
        public string? UserId { get; set; }
        public List<string>? RolesId { get; set; }
        public string? CreatedBy { get; set; }
    }
}
