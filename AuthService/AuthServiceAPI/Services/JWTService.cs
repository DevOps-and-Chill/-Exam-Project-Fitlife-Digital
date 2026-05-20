using AuthServiceAPI.Models;
using AuthServiceAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthServiceAPI.Services
{
    public class JWTService : IJWTService
    {
            private readonly IConfiguration _configuration;

            public JWTService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(Credential credential)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, credential.Id),
                    new Claim(ClaimTypes.Email, credential.Email)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]));

                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(
                        Convert.ToDouble(
                            _configuration["Jwt:DurationInMinutes"])),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler()
                    .WriteToken(token);
            }
    }
}
