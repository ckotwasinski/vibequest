using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.Service.Contracts
{
    public interface IEventCategoryService : IScopedDependency
    {
        Task<PagedList<CategoriesDto>> GetListAsync(PaginationParams userParams);

        Task<CategoriesDto> InsertEventcategory(CategoriesDto roleDto);

        Task<CategoriesDto> UpdateEventcategory(Guid id, CategoriesDto roleDto);

        Task<CategoriesDto> GetEventcategoryById(Guid roleId);

        Task<string> UploadEventCategoryImageAsync(IFormFile image, Guid id);

        Task DeleteEventcategoryById(Guid roleId);
    }
}
