using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using DP_backend.Configurations;
using DP_backend.Domain.Identity;

namespace DP_backend.Services
{
    public interface IJwtAuthService
    {
        public Task<string> GenerateToken(User user);
        public Task<IDictionary<string,object>> GetClaims(User user);

    }
    public class JwtAuthService : IJwtAuthService
    {
        private readonly JwtConfigurations _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManger;
        public JwtAuthService(IOptions<JwtConfigurations> jwtSettings, 
            ApplicationDbContext context, 
            UserManager<User> userManger)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
            _userManger = userManger;
        }

        public async Task<string> GenerateToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Claims = await GetClaims(user),
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.Add(_jwtSettings.Lifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Audience = _jwtSettings.Audience,
                    Issuer = _jwtSettings.Issuer

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<IDictionary<string, object>> GetClaims(User user)
        {
            var claims = new Dictionary<string, object>
            {
                { "Id", user.Id.ToString() },
                {"AccountId", user.AccountId.ToString() },
                {"Name", user.UserName },
                {"Email", user.Email },
            };
            try
            {
                var roles = await _userManger.GetRolesAsync(user);
                            foreach (var role in roles)
            {
                claims.Add(role, "true");
            }
            }
            catch
            {
                throw new InvalidOperationException($"Error to get User {user.Id} roles");
            }

            return claims;
        }
    }
}
