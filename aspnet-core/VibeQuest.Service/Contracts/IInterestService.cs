using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.Service.Contracts
{
    public interface IInterestService : IScopedDependency
    {
        Task<List<CategoriesDto>> GetInterestListAsync();

        Task InsertUserInterest(List<Guid> interests, Guid userId);

        Task<List<CategoriesDto>> GetInterestListByUserId(Guid userId);

        Task UpdateUserInterest(List<Guid> interests, Guid userId);
    }
}
