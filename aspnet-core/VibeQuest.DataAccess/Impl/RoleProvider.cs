using Microsoft.EntityFrameworkCore;
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
    public class RoleProvider : Repository<Roles, VibeQuestDbContext>, IRoleProvider
    {
        public RoleProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Guid> GetRoleIdByCode(string code)
        {
            return await _dbSet.Where(x => x.Code == code).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
