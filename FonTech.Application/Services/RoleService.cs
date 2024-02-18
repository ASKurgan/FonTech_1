using AutoMapper;
using FonTech.Application.Resources;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Dto.Role;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FonTech.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;

        public RoleService(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository,
                               IBaseRepository<UserRole> userRoleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
        {
            var user = await _userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == dto.Login);
             
            if (user == null) 
            {
               return new BaseResult<UserRoleDto>()
               { 
                 ErrorMessage = ErrorMessage.UserNotFound,
                 ErrorCode = (int)ErrorCodes.UserNotFound
               };
            }

            var roles = user.Roles.Select(x => x.Name).ToArray();
            if (roles.All(x => x != dto.RoleName))
            {
                var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.RoleName);
                if (role == null) 
                {
                    return new BaseResult<UserRoleDto>()
                    {
                        ErrorMessage = ErrorMessage.RoleNotFound,
                        ErrorCode = (int)ErrorCodes.RoleNotFound
                    };
                }
                var userRole = new UserRole()
                {
                  RoleId = role.Id, 
                  UserId = user.Id,
                };
               await _userRoleRepository.CreateAsync(userRole);
                return new BaseResult<UserRoleDto>()
                {
                    Data = new UserRoleDto()
                    {
                      Login = user.Login,
                      RoleName = role.Name,
                    }
                };
            }

            return new BaseResult<UserRoleDto>()
            { 
               ErrorMessage = ErrorMessage.UserAllreadyExistsThisRole,
               ErrorCode = (int)ErrorCodes.UserAllreadyExistsThisRole
            };
        }

        public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            Role? role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
            
            if (role != null)
            {
                return new BaseResult<RoleDto>()
                {
                  ErrorMessage = ErrorMessage.RoleAlreadyExists,
                  ErrorCode = (int)ErrorCodes.RoleAlreadyExists
                };
            }
            
           role = new Role()
            {
               Name = dto.Name,
           };

            await _roleRepository.CreateAsync(role);
            
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role)
            };


        }

        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
        {
            Role? role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }
            await _roleRepository.RemoveAsync(role);
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }

        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto)
        {
            Role? role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            role.Name = dto.Name;

            await _roleRepository.UpdateAsync(role);
           
            return new BaseResult<RoleDto>()
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }
    }
}
