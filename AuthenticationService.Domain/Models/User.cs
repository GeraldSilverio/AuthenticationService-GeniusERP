namespace AuthenticationService.Domain.Models
{
    public record User
    {
        public string UserId = string.Empty;
        public string Name = string.Empty;
        public string LastName = string.Empty;
        public string Identification = string.Empty;
        public string Business = string.Empty;
        public string Country = string.Empty;
        public string Address = string.Empty;
        public string BusinessId = string.Empty;
        public string CountryId = string.Empty;
        public string FirebaseId = string.Empty;
        
    }
}
