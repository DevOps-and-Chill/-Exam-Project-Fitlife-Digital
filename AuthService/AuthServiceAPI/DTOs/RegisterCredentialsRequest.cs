namespace AuthServiceAPI.DTOs
{
    public class RegisterCredentialsRequest
    {
        public string UserId { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
}
