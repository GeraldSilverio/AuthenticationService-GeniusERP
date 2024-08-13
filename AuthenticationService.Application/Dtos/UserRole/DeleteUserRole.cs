namespace AuthenticationService.Application.Dtos.UserRole
{
    public class DeleteUserRole
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public string? ModifiedBy {get; set; } 
    }
}
