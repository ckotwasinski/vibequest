using Microsoft.AspNetCore.Authorization;
using VibeQuest.Utility.JWT;
using VibeQuest.Utility.Permissions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Service;

namespace VibeQuest.Api.Handler
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUser _currentUser;

        public PermissionRequirementHandler(IPermissionService permissionService,
            ICurrentUser currentUser)
        {
            _permissionService = permissionService;
            _currentUser = currentUser;
        }

        protected async override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            var permissions = await _permissionService.GetRolePermissionsAsync(_currentUser.Role);
            if (permissions.Contains(requirement.PermissionName))
            {
                context.Succeed(requirement);
                return;
            }
        }   
    }
}
