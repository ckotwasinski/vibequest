using VibeQuest.Dto;
using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Service
{
    public interface IPermissionService : IScopedDependency
    {
        Task AddUpdatePermissions();
        Task<List<string>> GetRolePermissionsAsync(string roleCode);
        Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey);
        Task UpdateAsync(UpdatePermissionsDto input);
    }
}
