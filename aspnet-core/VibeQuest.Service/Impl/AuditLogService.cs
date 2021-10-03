using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Service.Impl
{
    public class AuditLogService : BaseService, IAuditLogService
    {
        private readonly IAuditLogProvider _auditLogProvider;
        public AuditLogService(IMapper Mapper,
            IAuditLogProvider auditLogProvider) : base(Mapper)
        {
            _auditLogProvider = auditLogProvider;
        }

        public async Task InsertAuditLog(AuditLogsDto log)
        {
            var auditLog = _mapper.Map<AuditLogs>(log);
            await _auditLogProvider.AddAsync(auditLog);
        }

        public async Task<PagedList<AuditLogsDto>> GetListAsync(PaginationParams userParams)
        {
            var query = _auditLogProvider.GetAuditLogsList().ProjectTo<AuditLogsDto>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                query = query.Where(x =>
                    x.ClientIpAddress.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.userName.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.HttpMethod.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.HttpStatusCode.ToString().ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Url.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.ExecutionTime.ToString().ToLower().Contains(userParams.Filter.ToLower())
                );
            }

            return await PagedList<AuditLogsDto>.CreateAsync(query, userParams);

        }

        public async Task<PagedList<AuditLogsDto>> GetErrorListAsync(PaginationParams userParams)
        {
            var query = _auditLogProvider.GetAuditLogsList().Where(x => x.Exception != null).ProjectTo<AuditLogsDto>(_mapper.ConfigurationProvider);
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                query = query.Where(x =>
                    x.ClientIpAddress.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.userName.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.HttpMethod.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.HttpStatusCode.ToString().ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Url.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.ExecutionTime.ToString().ToLower().Contains(userParams.Filter.ToLower())
                );
            }
            return await PagedList<AuditLogsDto>.CreateAsync(query, userParams);
        }
    }
}
