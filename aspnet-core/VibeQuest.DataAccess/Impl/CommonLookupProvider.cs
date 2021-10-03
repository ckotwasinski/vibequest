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
    public class CommonLookupProvider : Repository<CommonLookup, VibeQuestDbContext>, ICommonLookupProvider
    {
        public CommonLookupProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
