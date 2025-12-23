using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Books_2.Models
{
    public class AuthOptions
    {
        public const string Issuer = "AG"; // издатель токена
        public const string Audience = "APIclients"; // потребитель токена
        private const string Key = "!123&*superSecretKeyMustBeLoooooong!123&";// ключ шифрования
        public const int Lifetime = 365; // время жизни токена в днях

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }

        public static string GenerateToken(string username, bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, isAdmin ? "admin" : "user")
            };

            var jwt = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(Lifetime)),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
