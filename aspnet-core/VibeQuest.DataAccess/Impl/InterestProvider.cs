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
    public class InterestProvider : Repository<Categories, VibeQuestDbContext>, IInterestProvider
    {
        public InterestProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Categories>> GetInterestListAsync()
        {
            return await _unitOfWork.Context.Categories.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task InsertUserInterestAsync(Guid userId, Guid categoryId)
        {
            UserCategories userCategories = new UserCategories();
            userCategories.UserId = userId;
            userCategories.CategoryId = categoryId;
            userCategories.CreatedBy = userId;
            userCategories.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.Context.UserCategories.AddAsync(userCategories);        
        }

        public async Task<List<Categories>> GetUserCategoriesById(Guid userId)
        {
            var data = await (from category in _unitOfWork.Context.Categories
                              join userCategory in _unitOfWork.Context.UserCategories
                              on category.Id equals userCategory.CategoryId
                              where userCategory.UserId == userId && !userCategory.IsDeleted && !category.IsDeleted
                              select category).ToListAsync();
            return data;
        }

    }
}
