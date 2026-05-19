using System.ComponentModel.DataAnnotations;

namespace AuthServiceAPI.Models
{
    public class AuthUser
    {
        public string Id { get; init; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        
        //AO: No usecase for admin as of 18MAY26
        //public bool isAdmin { get; set; }
    }
}
