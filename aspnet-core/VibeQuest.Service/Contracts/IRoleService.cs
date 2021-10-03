using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.Service.Contracts
{
    public interface IRoleService : IScopedDependency
    {
        Task<PagedList<RolesDto>> GetListAsync(PaginationParams userParams);

        Task<RolesDto> InsertRole(RolesDto roleDto);

        Task<RolesDto> UpdateRole(Guid id, RolesDto roleDto);

        Task<RolesDto> GetRoleById(Guid roleId);

        Task DeleteRoleById(Guid roleId);

        Task<List<RolesDto>> GetRoleDropdownListAsync();
    }
}
