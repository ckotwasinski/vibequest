using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.Permissions;

namespace VibeQuest.Api.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(VibeQuestPermissions.User.Default)]
        [HttpGet]
        public async Task<PagedList<UsersDto>> GetUserList([FromQuery] PaginationParams pagination)
        {
            return await _userService.GetListAsync(pagination);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<UsersDto> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            return user;
        }

        [Authorize(VibeQuestPermissions.User.Create)]
        [HttpPost]
        public async Task<UsersDto> CreateUser(CreateUpdateUserDto userDto)
        {
            var user = await _userService.InsertUser(userDto);
            if (user == null)
                throw new UserFriendlyException(Constants.AppLocalization.EmailAlreadyExist);
            return user;
        }

        [Authorize(VibeQuestPermissions.User.Edit)]
        [HttpPut]
        [Route("{id}")]
        public async Task<UsersDto> UpdateUser(Guid id, CreateUpdateUserDto userDto)
        {
            var user = await _userService.UpdateUser(id, userDto);
            if (user == null)
                throw new UserFriendlyException(Constants.AppLocalization.EmailAlreadyExist);
            return user;
        }

        [Authorize(VibeQuestPermissions.User.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteUser(Guid id)
        {
            await _userService.DeleteUserById(id);
        }
    }
}
