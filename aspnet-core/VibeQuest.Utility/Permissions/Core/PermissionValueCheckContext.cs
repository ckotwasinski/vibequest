using VibeQuest.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public class PermissionValueCheckContext
    {
        public PermissionDefinition Permission { get; }

        public ClaimsPrincipal Principal { get; }

        public PermissionValueCheckContext(
            PermissionDefinition permission,
            ClaimsPrincipal principal)
        {
            Check.NotNull(permission, nameof(permission));

            Permission = permission;
            Principal = principal;
        }
    }
}
