using VibeQuest.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public class PermissionValuesCheckContext
    {
        public List<PermissionDefinition> Permissions { get; }

        public ClaimsPrincipal Principal { get; }

        public PermissionValuesCheckContext(
            List<PermissionDefinition> permissions,
            ClaimsPrincipal principal)
        {
            Check.NotNull(permissions, nameof(permissions));

            Permissions = permissions;
            Principal = principal;
        }
    }
}
