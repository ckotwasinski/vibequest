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
    public interface IEmailHistoryService : IScopedDependency
    {
        Task<PagedList<EmailHistoryDto>> GetListAsync(PaginationParams userParams);
    }
}
