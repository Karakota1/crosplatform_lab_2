using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Books_2.Services
{
    public class AuthService
    {
        private readonly AuthSettings _settings;

        // Жёстко закодированные пользователи
        private readonly List<(string Username, string Password)> _users = new()
        {
            ("user", "password"),
            ("volnovan", "praniki")
        };


        public AuthService(AuthSettings settings)
        {
            _settings = settings;
        }

        public string? Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user.Username == null)
                return null;

            return GenerateToken(user.Username);
        }

        private string GenerateToken(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Moderator") // только одна роль
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.LifetimeMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
