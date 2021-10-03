using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VibeQuest.DataAccess.Contracts
{
    public interface IAuditLogProvider : IRepository<AuditLogs>, IScopedDependency
    {
        IQueryable<AuditLogs> GetAuditLogsList();
    }
}
