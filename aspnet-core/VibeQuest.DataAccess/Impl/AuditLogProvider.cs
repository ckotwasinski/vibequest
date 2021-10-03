using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VibeQuest.DataAccess.Impl
{
    public class AuditLogProvider : Repository<AuditLogs, VibeQuestDbContext>, IAuditLogProvider
    {
        public AuditLogProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<AuditLogs> GetAuditLogsList()
        {
            var auditLogDetail = (from auditlogs in _dbSet
                              join u in _unitOfWork.Context.Users on auditlogs.UserId equals u.Id into users
                              from user in users.DefaultIfEmpty()
                              orderby auditlogs.ExecutionTime descending
                              select new AuditLogs
                              {
                                  UserId = auditlogs.UserId,
                                  ExecutionTime = auditlogs.ExecutionTime,
                                  ExecutionDuration = auditlogs.ExecutionDuration,
                                  ClientIpAddress = auditlogs.ClientIpAddress,
                                  BrowserInfo = auditlogs.BrowserInfo,
                                  HttpMethod = auditlogs.HttpMethod,
                                  Url = auditlogs.Url,
                                  Exception = auditlogs.Exception,
                                  HttpStatusCode = auditlogs.HttpStatusCode,
                                  Comments = auditlogs.Comments,
                                  Parameters = auditlogs.Parameters,
                                  Id = auditlogs.Id,
                                  userName = user.FullName
                              }).AsQueryable();
            return auditLogDetail;
        }
    }
}
