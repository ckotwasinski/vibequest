using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.DataAccess.Impl
{
    public class EmailHistoryProvider : Repository<EmailHistory, VibeQuestDbContext>, IEmailHistoryProvider
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EmailHistoryProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork,
            IServiceScopeFactory serviceScopeFactory) : base(unitOfWork)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task SaveEmailAsync(List<string> to, List<string> cc, List<string> bcc, string subject, string body, string fromMail, string Name, bool IsSent)
        {
            EmailHistory emailHistory = new EmailHistory();
            emailHistory.Name = Name;
            emailHistory.ToEmailAddress = String.Join(",", to);
            emailHistory.CCEmailAddress = String.Join(",", cc);
            emailHistory.Subject = subject;
            emailHistory.Body = body;
            emailHistory.BCCEmailAddress = String.Join(",", bcc);
            emailHistory.SentBy = 1;
            emailHistory.IsSent = IsSent;
            emailHistory.FromEmailAddress = fromMail;
            emailHistory.SentOn = DateTime.UtcNow;
            emailHistory.CreatedDate = DateTime.UtcNow;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<VibeQuestDbContext>();
                await base.AddAsync(emailHistory,db,true);
            }
        }
    }
}
