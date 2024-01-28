using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtKey;
        private readonly string _isseur;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options)
        {
            _jwtKey = options.Value.JwtKey;
            _isseur = options.Value.Issuer;
            _audience = options.Value.Audience;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var securityToken = 
                new JwtSecurityToken(_isseur, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
            
        }
    }
}
