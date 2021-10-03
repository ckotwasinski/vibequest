using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;

namespace VibeQuest.Service.Impl
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleProvider _roleProvider;

        public RoleService(IMapper Mapper,
           IRoleProvider roleProvider) : base(Mapper)
        {
            _roleProvider = roleProvider;
        }

        public async Task<PagedList<RolesDto>> GetListAsync(PaginationParams userParams)
        {
            var roles = _roleProvider.Get(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ProjectTo<RolesDto>(_mapper.ConfigurationProvider);
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                roles = roles.Where(x => x.Name.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Code.ToLower().Contains(userParams.Filter.ToLower()));
            }
            return await PagedList<RolesDto>.CreateAsync(roles, userParams);
        }
        public async Task<RolesDto> InsertRole(RolesDto roleDto)
        {
            if (_roleProvider.Get(x => x.Code.ToLower().Trim() == roleDto.Code.ToLower().Trim() && !x.IsDeleted).Any())
            {
                return null;
            }
            roleDto.CreatedDate = DateTime.UtcNow;
            var role = _mapper.Map<Roles>(roleDto);
            await _roleProvider.AddAsync(role);
            var result = _mapper.Map<RolesDto>(role);
            return result;
        }
        public async Task<RolesDto> UpdateRole(Guid id, RolesDto roleDto)
        {
            if (_roleProvider.Get(x => x.Code.ToLower().Trim() == roleDto.Code.ToLower().Trim() && x.Id != id && !x.IsDeleted).Any())
            {
                return null;
            }
            var role = await _roleProvider.GetByIdAsync(id);
            if (role != null)
            {
                role.Name = roleDto.Name;
                role.Code = roleDto.Code;
                role.UpdatedDate = DateTime.UtcNow;
                await _roleProvider.UpdateAsync(role);
            }
            var result = _mapper.Map<RolesDto>(role);
            return result;
        }
        public async Task<RolesDto> GetRoleById(Guid roleId)
        {
            var role = await _roleProvider.GetByIdAsync(roleId);
            return _mapper.Map<RolesDto>(role);

        }
        public async Task DeleteRoleById(Guid roleId)
        {
            var role = await _roleProvider.GetByIdAsync(roleId);
            if (role != null)
            {
                role.IsDeleted = true;
                role.DeletedDate = DateTime.UtcNow;
                await _roleProvider.UpdateAsync(role);
            }
        }
        public async Task<List<RolesDto>> GetRoleDropdownListAsync()
        {
            var roles = await _roleProvider.Get(x => x.IsDeleted == false).ToListAsync();
            return _mapper.Map<List<RolesDto>>(roles);
        }
    }
}
