using FonTech.Application.Resources;
using FonTech.Domain.Dto;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Serilog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using AutoMapper;
using System.Security.Claims;

namespace FonTech.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserToken> _userTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        public AuthService(IBaseRepository<User> userRepository, Serilog.ILogger logger, 
            IBaseRepository<UserToken> userTokenRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userTokenRepository = userTokenRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
           try 
            {
                User? user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);
                if (user == null) 
                {
                  return new BaseResult<TokenDto>()
                  { 
                     ErrorMessage = ErrorMessage.UserNotFound,
                     ErrorCode= (int)ErrorCodes.UserNotFound
                  };
                }

                
                if (!IsVerifyPassword(user.Password, dto.Password))
                {
                    return new BaseResult<TokenDto>()
                    {
                        ErrorMessage = ErrorMessage.PasswordIsWrong,
                        ErrorCode = (int)ErrorCodes.PasswordIsWrong
                    };
                }

                UserToken? userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Login),
                    new Claim(ClaimTypes.Role, "User")
                };
                string? accesToken = _tokenService.GenerateAccessToken(claims);
                string? refreshToken = _tokenService.GenerateRefreshToken();
                
                if (userToken == null)
                {
                    userToken = new UserToken()
                    {
                        UserId = user.Id,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                    };
                    await _userTokenRepository.CreateAsync(userToken);
                }
                else 
                {
                    userToken.RefreshToken = refreshToken;
                    userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                    await _userTokenRepository.UpdateAsync(userToken);
                }

                return new BaseResult<TokenDto>()
                {
                    Data = new TokenDto() 
                    {
                      RefreshToken = refreshToken,
                      AccesToken = accesToken
                    }
                };
            }
           catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
            
        }

        public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
        {
            throw new UnauthorizedAccessException("UnauthorizedAccessException");
            if (dto.Password != dto.PasswordConfirm)
            {
                return new BaseResult<UserDto>()
                { 
                  ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
                  ErrorCode =(int)ErrorCodes.PasswordNotEqualsPasswordConfirm,
                };
            }

            //try 
            //{
            //    User? user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);
            //    if (user != null) 
            //    {
            //        return new BaseResult<UserDto>()
            //        {
            //          ErrorMessage= ErrorMessage.UserAllreadyExists,
            //          ErrorCode =(int)ErrorCodes.UserAllreadyExists,
            //        };
            //    }

            //    string? hashUserPassword = HashPassword(dto.Password);
            //    user = new User()
            //    {
            //      Login = dto.Login,
            //      Password = hashUserPassword
            //    };

            //    await _userRepository.CreateAsync(user);
            //    return new BaseResult<UserDto>()
            //    {
            //        Data = _mapper.Map<UserDto>(user)
            //    };
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex, ex.Message);
            //    return new BaseResult<UserDto>()
            //    {
            //        ErrorMessage = ErrorMessage.InternalServerError,
            //        ErrorCode = (int)ErrorCodes.InternalServerError,
            //    };
            //}

        }

        private string HashPassword(string password)
        {
            byte[]? bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password)); 
            return Convert.ToBase64String(bytes);
        }

        private bool IsVerifyPassword(string userPasswordHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return hash == userPasswordHash;
        }
    }


}
