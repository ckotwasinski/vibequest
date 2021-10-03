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
    public class PermissionProvider : Repository<PermissionGrants, VibeQuestDbContext>, IPermissionProvider
    {
        public PermissionProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<PermissionGrants>> GetPermissionGrantsByProviderKey(Guid providerKey)
        {
            return await _dbSet.Where(x => x.ProviderKey == providerKey && !x.IsDeleted).ToListAsync();
        }
    }
}
