namespace AuthServiceAPI.DTOs
{
    public class ValidateCredentialsRequestDTO
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
