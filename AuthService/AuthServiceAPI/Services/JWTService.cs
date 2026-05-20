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
            
            //AO: Claims are data about the user (from credential-object in this case) thats included in the token. 
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, credential.Id),
                    new Claim(ClaimTypes.Email, credential.Email)
                };

            //AO: Get the secret key and save it as a local variable. 
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]));

            //AO: Use the key and the specified algorithm to create the signature of the token.This is the configuration for encrypting the token
                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);
            
            //AO: Build the token. Issuer is who made the token and audience is who its made for.  Altså adds expiration for the token
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(
                        Convert.ToDouble(
                            _configuration["Jwt:DurationInMinutes"])),
                    signingCredentials: creds);

            //AO: token is made into a string
                return new JwtSecurityTokenHandler()
                    .WriteToken(token);
            }
    }
}
