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
    public class UserCategoriesProvider : Repository<UserCategories, VibeQuestDbContext>, IUserCategoriesProvider
    {
        public UserCategoriesProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
