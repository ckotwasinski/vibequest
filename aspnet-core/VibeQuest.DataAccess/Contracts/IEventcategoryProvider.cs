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
    public interface IEventcategoryProvider : IRepository<Categories>, IScopedDependency
    {
        Task<Guid> GetEventCategoryIdByCode(string code);
    
    }
}
