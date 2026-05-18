namespace AuthServiceAPI.Models
{
    public class Credential
    {
        public string Id { get; init; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        
        //AO: No usecase for admin as of 18MAY26
        //public bool isAdmin { get; set; }
    }
}
