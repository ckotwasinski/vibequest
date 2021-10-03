using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;

namespace VibeQuest.DataAccess.Impl
{
    public class EventcategoryProvider : Repository<Categories, VibeQuestDbContext>, IEventcategoryProvider
    {
        public EventcategoryProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Guid> GetEventCategoryIdByCode(string code)
        {
            return await _dbSet.Where(x => x.Code == code).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
