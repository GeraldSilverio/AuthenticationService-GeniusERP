namespace AuthenticationService.Api.Domain
{
    public class User
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Identification { get; set; }
        public string? Business { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string? BusinessId { get; set; }
        public string? CountryId { get; set; }
        public string? FirebaseId { get; set; }
    }
}
