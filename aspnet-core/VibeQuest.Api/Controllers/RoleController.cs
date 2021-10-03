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
    [Route("role")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(VibeQuestPermissions.Role.Default)]
        [HttpGet]
        public async Task<PagedList<RolesDto>> GetRoleList([FromQuery] PaginationParams pagination)
        {
            return await _roleService.GetListAsync(pagination);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<RolesDto> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleById(id);
            return role;
        }

        [Authorize(VibeQuestPermissions.Role.Create)]
        [HttpPost]
        public async Task<RolesDto> CreateRole(RolesDto rolesDto)
        {
            var role = await _roleService.InsertRole(rolesDto);
            if (role == null)
                throw new UserFriendlyException(Constants.AppLocalization.RoleAlreadyExist);
            return role;
        }

       [Authorize(VibeQuestPermissions.Role.Edit)]
        [HttpPut]
        [Route("{id}")]
        public async Task<RolesDto> UpdateRole(Guid id, RolesDto rolesDto)
        {
            var role = await _roleService.UpdateRole(id, rolesDto);
            if (role == null)
                throw new UserFriendlyException(Constants.AppLocalization.RoleAlreadyExist);
            return role;
        }

        [Authorize(VibeQuestPermissions.Role.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteRole(Guid id)
        {
            await _roleService.DeleteRoleById(id);
        }

        [Authorize]
        [Route("dropdown-list")]
        [HttpGet]
        public async Task<List<RolesDto>> GetRoleListDropdown()
        {
            return await _roleService.GetRoleDropdownListAsync();
        }
    }
}
