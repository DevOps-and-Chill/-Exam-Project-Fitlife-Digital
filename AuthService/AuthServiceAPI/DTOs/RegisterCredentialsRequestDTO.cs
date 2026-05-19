namespace AuthServiceAPI.DTOs
{
    /// <summary>
    /// string UserID (Guid from UserService), string Email, string Password
    /// </summary>
    public class RegisterCredentialsRequestDTO
    {
        public string UserId { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}

