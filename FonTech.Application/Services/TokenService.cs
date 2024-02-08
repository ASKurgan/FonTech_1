using FonTech.Application.Resources;
using FonTech.Domain.Dto;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using FonTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
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
        private readonly IBaseRepository<User> _userRepository;
        private readonly string _jwtKey;
        private readonly string _isseur;
        private readonly string _audience;

        public TokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepository)
        {
            _jwtKey = options.Value.JwtKey;
            _isseur = options.Value.Issuer;
            _audience = options.Value.Audience;
            _userRepository = userRepository;
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

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
               ValidateAudience = true,
               ValidateIssuer = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
               ValidateLifetime = true,
               ValidAudience = _audience,
               ValidIssuer = _isseur
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters,out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException(ErrorMessage.InvalidToken);
            }
            return claimsPrincipal;
        }

        public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
        {
            string? accessToken = dto.AccesToken;
            string? refreshToken = dto.RefreshToken;

            ClaimsPrincipal? claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
            string? userName = claimsPrincipal.Identity?.Name;
            User? user = await _userRepository.GetAll()
                       .Include(x => x.UserToken)
                       .FirstOrDefaultAsync(x => x.Login == userName);

            if (user == null || user.UserToken.RefreshToken != refreshToken ||
                user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow ) 
            {
                return new BaseResult<TokenDto>()
                { 
                    ErrorMessage = ErrorMessage.InvalidClientRequest
                };
            }
            var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            user.UserToken.RefreshToken = newRefreshToken;
            await _userRepository.UpdateAsync(user);

            return new BaseResult<TokenDto>
            {
                Data = new TokenDto()
                { 
                    RefreshToken = newRefreshToken,
                    AccesToken = newAccessToken,
                }
            };
        }
    }
}
