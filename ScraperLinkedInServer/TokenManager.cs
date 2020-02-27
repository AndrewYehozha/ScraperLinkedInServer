using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;

namespace ScraperLinkedInServer
{
    public class TokenManager
    {
        private readonly static string Secret = "LGNSELdlknKNn837wWDL2ZzfwefasdWdQkFHiovnspaS";
        public static string GenerateToken(AccountViewModel account, string role)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(Claims.AccountId, account.Id.ToString()),
                    new Claim(Claims.FirstName, account.FirstName),
                    new Claim(Claims.LastName, account.LastName),
                    new Claim(Claims.Email, account.Email),
                    new Claim(Claims.IsAdmin, (account.Role == Roles.Admin).ToString()),
                    new Claim(ClaimTypes.Role, Roles.WindowsService == role ? role : account.Role)
            });
            
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(Secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            DateTime expiresDate = (account.Role == Roles.Admin) ? DateTime.UtcNow.AddDays(7)
                                    : (role == Roles.WindowsService) ? DateTime.UtcNow.AddDays(2)
                                    : DateTime.UtcNow.AddDays(1);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "ScraperLinkedInServer",
                Audience = "ScraperLinkedInServer",
                Subject = identity,
                IssuedAt = DateTime.UtcNow,
                Expires = expiresDate,
                SigningCredentials = signingCredentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}