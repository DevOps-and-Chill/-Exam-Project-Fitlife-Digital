namespace AuthServiceAPI.Models
{
    /// <summary>
    /// String id (from guid), string email, string passwordHash
    /// </summary>
    public class Credential
    {
        public string Id { get; init; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        
        //AO: No usecase for admin as of 18MAY26
        //public bool isAdmin { get; set; }

        public Credential(string  id, string email, string passwordHash)
        {
            Id = id;
            Email = email;
            SetPasswordHash(passwordHash);
        }

        public void SetPasswordHash(string hashedPassword)
        {
            PasswordHash = hashedPassword;
        }
    }
}
