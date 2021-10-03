using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class GetPermissionListResultDto
    {
        public List<PermissionGroupDto> Groups { get; set; }
    }

    public class PermissionGroupDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<PermissionGrantInfoDto> Permissions { get; set; }
    }

    public class PermissionGrantInfoDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentName { get; set; }
        public bool IsGranted { get; set; }
        public List<string> AllowedProviders { get; set; }
    }

    public class UpdatePermissionsDto
    {
        public string ProviderKey { get; set; }
        public string ProviderName { get; set; }
        public UpdatePermissionDto[] Permissions { get; set; }
    }

    public class UpdatePermissionDto
    {
        public string Name { get; set; }

        public bool IsGranted { get; set; }
    }
}
