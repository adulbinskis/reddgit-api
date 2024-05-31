using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.Application.Identity.Services
{
    public class TokenService
    {
        // Adjustexpiration if needed
        private const int ExpirationMinutes = 20;
        private readonly ILogger<TokenService> _logger;
        private readonly IConfiguration _configuration;

        public TokenService(ILogger<TokenService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string CreateToken(ApplicationUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

            var token = CreateJwtToken(
                  CreateClaims(user),
                  CreateSigningCredentials(),
                  expiration
              );
            var tokenHandler = new JwtSecurityTokenHandler();

            _logger.LogInformation("JWT Token created");

            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            return new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiration,
                signingCredentials: credentials
            );
        }

        private List<Claim> CreateClaims(ApplicationUser user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:JwtRegisteredClaimNamesSub"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var key = _configuration["Jwt:Key"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
