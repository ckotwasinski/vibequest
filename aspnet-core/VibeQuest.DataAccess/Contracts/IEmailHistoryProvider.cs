using System.Collections.Generic;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.DataAccess.Contracts
{
    public interface IEmailHistoryProvider : IRepository<EmailHistory>, IScopedDependency
    {
        Task SaveEmailAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string body, string fromMail, string Name, bool IsSent);
    }
}
