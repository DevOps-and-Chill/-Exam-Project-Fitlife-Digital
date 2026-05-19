namespace AuthServiceAPI.DTOs
{
    public class ValidateTokenResponseDTO
    {
        public bool IsValid { get; set; }
        public string? UserId { get; set; }
    }
}
