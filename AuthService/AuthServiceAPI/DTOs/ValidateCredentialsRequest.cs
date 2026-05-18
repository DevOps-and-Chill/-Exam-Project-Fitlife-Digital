namespace AuthServiceAPI.DTOs
{
    public class ValidateCredentialsRequest
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
