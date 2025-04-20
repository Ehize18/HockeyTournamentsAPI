using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HockeyTournamentsAPI.Infrastructure.Jwt.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HockeyTournamentsAPI.Infrastructure.Jwt
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public string GenerateToken(string email, string role)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, role)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiredHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
