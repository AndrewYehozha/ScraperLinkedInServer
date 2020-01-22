using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ScraperLinkedInServer
{
    public class TokenManager
    {
        private readonly static string Secret = "LGNSELdlknKNn837=wWDL2Z+zfwefasdWdQkFHiovnspa==S";
        public static string GenerateToken(string userId, string role, string email, string password)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim("UserId", userId),
                    new Claim("Role", role),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Password", password)
            });
            byte[] key = Convert.FromBase64String(Secret);
            var securityKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "ScraperLinkedInServer",
                Audience = "ScraperLinkedInServer",
                Subject = identity,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signingCredentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}