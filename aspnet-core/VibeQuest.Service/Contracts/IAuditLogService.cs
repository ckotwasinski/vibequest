using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Service.Contracts
{
    public interface IAuditLogService : IScopedDependency
    {
        Task InsertAuditLog(AuditLogsDto log);

        Task<PagedList<AuditLogsDto>> GetListAsync(PaginationParams userParams);

        Task<PagedList<AuditLogsDto>> GetErrorListAsync(PaginationParams userParams);
    }
}
