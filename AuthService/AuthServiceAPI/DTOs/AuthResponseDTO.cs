namespace AuthServiceAPI.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
