using MABS.Application.Common.Interfaces.Authentication;
using MABS.Domain.Models.ProfileModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MABS.Infrastructure.Common.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly ILogger<JwtTokenGenerator> _logger;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(ILogger<JwtTokenGenerator> logger, IOptions<JwtSettings> jwtOptions)
        {
            _logger = logger;
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(Profile profile)
        {
            _logger.LogInformation($"Creating Jwt Token for {profile.Email}.");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, profile.UUID.ToString()),
                new Claim(ClaimTypes.Name, profile.Email)
            };

            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                SecurityAlgorithms.HmacSha256Signature
            );

            var token = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: signingCredentials
            );

            var strToken = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogDebug($"Created login Jwt Token for {profile.Email} ({strToken}).");

            return strToken;
        }
    }
}
