﻿using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Database;

namespace ScraperLinkedInServer
{
    public class TokenManager
    {
        private readonly static string Secret = "LGNSELdlknKNn837wWDL2ZzfwefasdWdQkFHiovnspaS";
        public static string GenerateToken(Account account)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(Claims.AccountId, account.Id.ToString()),
                    new Claim(Claims.FirstName, account.FirstName),
                    new Claim(Claims.LastName, account.LastName),
                    new Claim(Claims.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role)
            });
            
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(Secret));
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