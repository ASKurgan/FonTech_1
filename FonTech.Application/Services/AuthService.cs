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

namespace FonTech.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        public AuthService(IBaseRepository<User> userRepository, Serilog.ILogger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
            if ()
            {
                
            }
            throw new NotImplementedException();
        }

        public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
        {
            if (dto.Password != dto.PasswordConfirm)
            {
                return new BaseResult<UserDto>()
                { 
                  ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
                  ErrorCode =(int)ErrorCodes.PasswordNotEqualsPasswordConfirm,
                };
            }

            try 
            {
                User? user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);
                if (user != null) 
                {
                    return new BaseResult<UserDto>()
                    {
                      ErrorMessage= ErrorMessage.UserAllreadyExists,
                      ErrorCode =(int)ErrorCodes.UserAllreadyExists,
                    };
                }

                string? hashUserPassword = HashPassword(dto.Password);
                user = new User()
                {
                  Login = dto.Login,
                  Password = hashUserPassword
                };

                await _userRepository.CreateAsync(user);
                return new BaseResult<UserDto>()
                {
                    Data = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }

        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password)); 
            return BitConverter.ToString(bytes).ToLower();
        }
    }


}
