using AuthServiceAPI.Models;

namespace AuthServiceAPI.Services.Interfaces
{
    public interface IJWTService
    {
        public string GenerateToken(Credential credential);
    }
}
