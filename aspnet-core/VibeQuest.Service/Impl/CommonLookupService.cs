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
    public class CommonLookupService : BaseService, ICommonLookupService
    {
       
        private readonly ICommonLookupProvider _commonLookupProvider;

        public CommonLookupService(IMapper Mapper, ICommonLookupProvider commonLookupProvider) : base(Mapper)
        {
            _commonLookupProvider = commonLookupProvider;
        }

        public async Task<CommonLookupDto> InsertAsync(CommonLookupDto lookupDto)
        {
            if (_commonLookupProvider.Get(x => x.ConfigKey.ToLower().Equals(lookupDto.ConfigKey.ToLower().Trim())
            && x.ConfigName.ToLower().Equals(lookupDto.ConfigName.ToLower().Trim())).Any())
            {
                return null;
            }
            var commonLookup = _mapper.Map<CommonLookup>(lookupDto);
            await _commonLookupProvider.AddAsync(commonLookup);
            return lookupDto;
        }

        public async Task<PagedList<CommonLookupDto>> GetListAsync(PaginationParams userParams)
        {
            var query = _commonLookupProvider.Get(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ProjectTo<CommonLookupDto>(_mapper.ConfigurationProvider);
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                query = query.Where(x => x.ConfigKey.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.ConfigName.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.ConfigValue.ToLower().Contains(userParams.Filter.ToLower())
                );
            }

            return await PagedList<CommonLookupDto>.CreateAsync(query, userParams);

        }

        public async Task<List<CommonLookupDto>> GetCommonLookupByConfigNameAsync(string configName)
        {
            var query = _commonLookupProvider.Get(x => x.IsDeleted == false && x.ConfigName == configName).ProjectTo<CommonLookupDto>(_mapper.ConfigurationProvider);

            return await query.ToListAsync();
        }

        public async Task<CommonLookupDto> GetCommonLookupById(Guid lookupId)
        {
            var commonLookup = await _commonLookupProvider.GetByIdAsync(lookupId);
            return _mapper.Map<CommonLookupDto>(commonLookup);

        }

        public async Task DeleteCommonLookupById(Guid lookupId)
        {
            var commonLookup = await _commonLookupProvider.GetByIdAsync(lookupId);
            if (commonLookup != null)
            {
                commonLookup.IsDeleted = true;
                commonLookup.DeletedDate = DateTime.UtcNow;
                await _commonLookupProvider.UpdateAsync(commonLookup);
            }
        }

        public async Task<CommonLookupDto> UpdateCommonLookup(Guid id, CommonLookupDto lookupDto)
        {
            if (_commonLookupProvider.Get(x => x.ConfigKey.ToLower().Equals(lookupDto.ConfigKey.ToLower().Trim())
             && x.ConfigName.ToLower().Equals(lookupDto.ConfigName.ToLower().Trim()) && x.Id != id).Any())
            {
                return null;
            }
            var commonLookup = await _commonLookupProvider.GetByIdAsync(id);
            if (commonLookup != null)
            {
                commonLookup.ConfigKey = lookupDto.ConfigKey;
                commonLookup.ConfigName = lookupDto.ConfigName;
                commonLookup.ConfigValue = lookupDto.ConfigValue;
                commonLookup.IsActive = lookupDto.IsActive;
                commonLookup.Description = lookupDto.Description;
                commonLookup.DisplayOrder = lookupDto.DisplayOrder;
                await _commonLookupProvider.UpdateAsync(commonLookup);
            }
            var result = _mapper.Map<CommonLookupDto>(commonLookup);
            return result;
        }

    }
}
