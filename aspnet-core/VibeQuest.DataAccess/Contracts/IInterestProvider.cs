using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.DataAccess.Contracts
{
    public interface IInterestProvider : IRepository<Categories>, IScopedDependency
    {
        Task<List<Categories>> GetInterestListAsync();

        Task InsertUserInterestAsync(Guid userId, Guid categoryId);

        Task<List<Categories>> GetUserCategoriesById(Guid userId);
    }
}
