namespace AuthenticationService.Application.Dtos.Account
{
    public class RegisterUserDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
