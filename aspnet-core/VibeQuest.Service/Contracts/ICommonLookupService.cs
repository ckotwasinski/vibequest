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
    public interface ICommonLookupService : IScopedDependency
    {
        Task<CommonLookupDto> InsertAsync(CommonLookupDto lookupDto);

        Task<CommonLookupDto> UpdateCommonLookup(Guid id, CommonLookupDto emailTemplateDto);

        Task<CommonLookupDto> GetCommonLookupById(Guid id);

        Task<PagedList<CommonLookupDto>> GetListAsync(PaginationParams userParams);

        Task<List<CommonLookupDto>> GetCommonLookupByConfigNameAsync(string configName);

        Task DeleteCommonLookupById(Guid lookupId);
    }
}
