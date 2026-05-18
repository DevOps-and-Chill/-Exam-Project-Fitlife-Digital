namespace AuthServiceAPI.DTOs
{
    public class ValidateTokenResponse
    {
        public bool IsValid { get; set; }
        public string? UserId { get; set; }
    }
}
