using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service;
using VibeQuest.Utility.Extensions;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.JWT;

namespace VibeQuest.Api.Controllers
{
    [Route("permissions")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUser _currentUser;

        public PermissionController(IPermissionService permissionService,
               ICurrentUser currentUser)
        {
            _permissionService = permissionService;
            _currentUser = currentUser;
        }

       [Authorize]
        [HttpGet]
        [Route("{providerKey}")]
        public async Task<GetPermissionListResultDto> GetAsync(string providerKey)
        {
            string providerName = Constants.ProviderName.Role;
            return await _permissionService.GetAsync(providerName, providerKey);
        }

        [Authorize]
        [HttpPost]
        public async Task InsertUpdateAsync(UpdatePermissionsDto input)
        {
            input.ProviderName = Constants.ProviderName.Role;
            await _permissionService.UpdateAsync(input);
        }

        [HttpGet]
        [Route("by-user")]
        public async Task<string[]> GetPermissions()
        {
            var roleCode = _currentUser.Role;
            if (roleCode.IsNullOrWhiteSpace())
                return Array.Empty<string>();

            var permissions = await _permissionService.GetRolePermissionsAsync(roleCode);
            return permissions.ToArray();
        }
    }
}
