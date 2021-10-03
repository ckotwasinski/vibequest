using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Impl;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.InMemoryCaching;
using VibeQuest.Utility.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VibeQuest.Service
{
    public class PermissionService : BaseService, IPermissionService
    {
        private IPermissionDefinitionManager _permissionDefinitionManager { get; }
        private readonly IPermissionProvider _permissionProvider;
        private readonly IRoleProvider _roleProvider;
        private readonly ICacheProvider _cacheProvider;

        private static readonly SemaphoreSlim GetPermissionSemaphore = new SemaphoreSlim(1, 1);

        public PermissionService(IMapper Mapper,
            IPermissionDefinitionManager permissionDefinitionManager, 
            IPermissionProvider permissionProvider,
            IRoleProvider roleProvider,
            ICacheProvider cacheProvider) : base(Mapper)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _permissionProvider = permissionProvider;
            _roleProvider = roleProvider;
            _cacheProvider = cacheProvider;
        }

        public async Task AddUpdatePermissions()
        {
            var groups = _permissionDefinitionManager.GetGroups();
            var sadminRoleId = await _roleProvider.GetRoleIdByCode(Constants.Roles.Sadmin);
            if (sadminRoleId == default(Guid))
                throw new UserFriendlyException("sadmin user not found!");

            var sadminPermissions = await _permissionProvider.GetPermissionGrantsByProviderKey(sadminRoleId);
            var permissionDefinitions = new List<PermissionDefinition>();

            foreach (var group in groups)
            {
                var permissions = group.GetPermissionsWithChildren();
                if(permissions != null)
                {
                    permissionDefinitions.AddRange(permissions);
                }
            }

            var permissionsToDelete = sadminPermissions.Where(x => !permissionDefinitions.Any(y => y.Name == x.Name)).ToList();
            foreach (var deletePermission in permissionsToDelete)
            {
                await _permissionProvider.DeleteAsync(deletePermission, false);
            }
            if (permissionsToDelete.Any())
                await _permissionProvider.SaveChangeAsync();

            var addPermissions = permissionDefinitions.Where(x => !sadminPermissions.Any(y => y.Name == x.Name)).ToList();
            foreach (var permission in addPermissions)
            {
                var permissionGrant = new PermissionGrants
                {
                    Id = Guid.NewGuid(),
                    Name = permission.Name,
                    ProviderName = Constants.ProviderName.Role,
                    ProviderKey = sadminRoleId,
                    IsDeleted = false
                };
                await _permissionProvider.AddAsync(permissionGrant, false);
            }
            if (addPermissions.Any())
                await _permissionProvider.SaveChangeAsync();
        }

        public async Task<List<string>> GetRolePermissionsAsync(string roleCode)
        {
            var roleId = await _roleProvider.GetRoleIdByCode(roleCode);
            if (roleId == default(Guid))
                throw new UserFriendlyException("role not found!");

            return await GetCachedResponse(roleId.ToString(), GetPermissionSemaphore, () => _permissionProvider.GetPermissionGrantsByProviderKey(roleId));
        }

        private async Task<List<string>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphore, Func<Task<List<PermissionGrants>>> func)
        {
            var permissions = _cacheProvider.GetFromCache<List<string>>(cacheKey);
            if (permissions != null) return permissions;

            try
            {
                await semaphore.WaitAsync();
                permissions = _cacheProvider.GetFromCache<List<string>>(cacheKey); // Recheck to make sure it didn't populate before entering semaphore
                if (permissions != null) return permissions;

                var permissionGrants = await func();
                permissions = permissionGrants.Select(x => x.Name).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _cacheProvider.SetCache(cacheKey, permissions, cacheEntryOptions);
            }
            finally
            {
                semaphore.Release();
            }

            return permissions;
        }

        public async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            var result = new GetPermissionListResultDto
            {
                Groups = new List<PermissionGroupDto>()
            };

            var groups = _permissionDefinitionManager.GetGroups();
            var grantedPermission = await _permissionProvider.GetPermissionGrantsByProviderKey(new Guid(providerKey));

            foreach (var group in groups)
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName,
                    Permissions = new List<PermissionGrantInfoDto>()
                };

                foreach (var permission in group.GetPermissionsWithChildren())
                {
                    if (!permission.IsEnabled)
                    {
                        continue;
                    }

                    if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
                    {
                        continue;
                    }

                    var grantInfoDto = new PermissionGrantInfoDto
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName,
                        ParentName = permission.Parent?.Name,
                        AllowedProviders = permission.Providers
                    };

                    var grantInfo = grantedPermission.Where(x => x.Name == permission.Name && x.ProviderName == providerName).FirstOrDefault();

                    grantInfoDto.IsGranted = grantInfo != null && !grantInfo.IsDeleted;
                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }

        public async Task UpdateAsync(UpdatePermissionsDto input)
        {
            var grantedPermission = await _permissionProvider.GetPermissionGrantsByProviderKey(new Guid(input.ProviderKey));
            var deletePermissions = grantedPermission.Where(x => input.Permissions.Any(y => !y.IsGranted && y.Name == x.Name)).ToList();
            await _permissionProvider.DeleteAsync(deletePermissions);

            var addPermissions = input.Permissions.Where(x => x.IsGranted && !grantedPermission.Any(y => y.Name == x.Name));
            foreach (var permission in addPermissions)
            {
                var permissionGrant = new PermissionGrants
                {
                    Id = Guid.NewGuid(),
                    Name = permission.Name,
                    ProviderName = Constants.ProviderName.Role,
                    ProviderKey = new Guid(input.ProviderKey),
                    IsDeleted = false
                };
                await _permissionProvider.AddAsync(permissionGrant, false);
            }
            if (addPermissions.Any())
                await _permissionProvider.SaveChangeAsync();
        }
    }
}
