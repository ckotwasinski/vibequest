using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.DataAccess.Impl
{
    public class EmailTemplatesProvider : Repository<EmailTemplates, VibeQuestDbContext>, IEmailTemplatesProvider
    {
        public EmailTemplatesProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
