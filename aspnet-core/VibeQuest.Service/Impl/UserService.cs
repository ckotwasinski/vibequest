using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.Service.Impl
{
    public class UserService : BaseService, IUserService 
    {
        private readonly IUserProvider _userProvider;
        private readonly IUserRoleProvider _userRoleProvider;
        private readonly IRoleProvider _roleProvider;
        private readonly IConfiguration _appConfiguration;
        public string apiImageUrl { get => _appConfiguration[Constants.BaseUrl]; }
        public UserService(IMapper Mapper,
            IUserProvider userProvider,
            IUserRoleProvider userRoleProvider,
            IRoleProvider roleProvider,
            IConfiguration appConfiguration) : base(Mapper)
        {
            _userProvider = userProvider;
            _userRoleProvider = userRoleProvider;
            _roleProvider = roleProvider;
            _appConfiguration = appConfiguration;
        }

        public async Task DeleteUserById(Guid userId)
        {
            var user = await _userProvider.GetByIdAsync(userId);
            if (user != null)
            {
                await _userProvider.DeleteAsync(user);
            }
        }

        public async Task<PagedList<UsersDto>> GetListAsync(PaginationParams userParams)
        {
           
            var users = _userProvider.GetUsersList().ProjectTo<UsersDto>(_mapper.ConfigurationProvider);
            
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                users = users.Where(x => x.FullName.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Email.ToLower().Contains(userParams.Filter.ToLower())
                );
            }

            return await PagedList<UsersDto>.CreateAsync(users, userParams);
        }

        public async Task<UsersDto> GetUserById(Guid userId)
        {
            
               var baseUrl = apiImageUrl + "{0}";
            var user = await _userProvider.GetUserById(userId);
            var userDto = _mapper.Map<UsersDto>(user);
            if (!string.IsNullOrEmpty(userDto.ProfilePhoto))
            {
                userDto.ProfilePhotoFullPath = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, userDto.ProfilePhoto));
                userDto.ProfilePhoto = userDto.ProfilePhoto;
            }
            return userDto;
        }

        public async Task<UsersDto> InsertUser(CreateUpdateUserDto userDto)
        {
            UsersDto result = null;
            if (!_userProvider.Get(x => x.Email.ToLower().Trim() == userDto.Email.ToLower().Trim() && !x.IsDeleted).Any())
            {
                if (userDto != null)
                {
                    var hashPassword = PBKDF2.HashPassword(userDto.Password);
                    userDto.PasswordSalt = hashPassword.Item1;
                    userDto.Password = hashPassword.Item2;
                    var user = _mapper.Map<Users>(userDto);
                    await _userProvider.AddAsync(user);
                    var userId = user.Id;
                    var roleId = _roleProvider.Get(x => x.Code == user.RoleCode).Select(x => x.Id).FirstOrDefault();
                    var userRole = new UserRoles();
                    userRole.UserId = userId;
                    userRole.RoleId = roleId;
                    await _userRoleProvider.AddAsync(userRole);
                    result = _mapper.Map<UsersDto>(user);
                }
            }
            return result;
        }

        public async Task<UsersDto> UpdateUser(Guid id, CreateUpdateUserDto userDto)
        {
            UsersDto result = null;
            if (!_userProvider.Get(x => x.Email.ToLower().Trim() == userDto.Email.ToLower().Trim() && x.Id != id && !x.IsDeleted).Any())
            {
                var user = await _userProvider.GetByIdAsync(id);
                if (user != null)
                {
                    user.FullName = userDto.FullName;
                    user.IsActive = userDto.IsActive;
                    user.RoleCode = userDto.RoleCode;
                    user.ProfilePhoto = userDto.ProfilePhoto;
                    await _userProvider.UpdateAsync(user);

                    var userId = user.Id;
                    var roleId = _roleProvider.Get(x => x.Code == user.RoleCode).Select(x => x.Id).FirstOrDefault();
                    var userRole = _userRoleProvider.Get(x => x.UserId == userId).FirstOrDefault();
                    if (userRole != null)
                    {
                        userRole.RoleId = roleId;
                        await _userRoleProvider.UpdateAsync(userRole);
                    }
                    result = _mapper.Map<UsersDto>(user);
                }
            }
            return result;
        }
    }
}
