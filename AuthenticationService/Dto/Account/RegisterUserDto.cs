namespace AuthenticationService.Application.Dtos.Account
{
    public class RegisterUserDto
    {
        public string? Names { get; set; }
        public string? LastNames { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Identification { get; set; }
        public string? PhotoUrl { get; set; }
        public char Gender { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public int CountryId { get; set; }
        public int TypeIdentificationId { get; set; }
        public string? BusinessId { get; set; }
        public string? Password { get; set; }
        public List<string>? RolesId { get; set; }
        public string? CreateBy { get; set; }
    }
}
