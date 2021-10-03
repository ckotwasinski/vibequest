using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.Service.Impl
{
    public class EventCategoryService : BaseService, IEventCategoryService
    {
        private readonly IEventcategoryProvider _eventCategoryProvider;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IConfiguration _appConfiguration;
        public EventCategoryService(IMapper Mapper,
           IEventcategoryProvider eventCategoryProvider,
            IWebHostEnvironment hostingEnvironment,
           IConfiguration appConfiguration) : base(Mapper)
        {
            _eventCategoryProvider = eventCategoryProvider;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = appConfiguration;
        }
        public string apiImageUrl { get => _appConfiguration[Constants.BaseUrl]; }
        public async Task<string> UploadEventCategoryImageAsync(IFormFile image, Guid id)
        {
            string filename = string.Empty;
            var baseUrl = apiImageUrl + "{0}";
            var categories = await _eventCategoryProvider.Get(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (categories != null)
            {
                Categories model = _mapper.Map<Categories>(categories);
                if (image != null)
                {
                    filename = await FileHelper.CreateFileAsync(image, _hostingEnvironment.WebRootPath, Constants.InterestThumbnailImagesPath);
                    model.Photo = filename;
                    await _eventCategoryProvider.UpdateAsync(model);
                    string imagePath = string.Format(baseUrl, Constants.GetUrl(Constants.InterestThumbnailImagesPath, model.Photo));
                    return imagePath;
                }
            }
            return null;
        }

        public async Task DeleteEventcategoryById(Guid eventCategoryId)
        {
            var eventCategory = await _eventCategoryProvider.GetByIdAsync(eventCategoryId);
            if (eventCategory != null)
            {
                eventCategory.IsDeleted = true;
                eventCategory.DeletedDate = DateTime.UtcNow;
                await _eventCategoryProvider.UpdateAsync(eventCategory);
            }
        }

        public async Task<CategoriesDto> GetEventcategoryById(Guid eventCategoryId)
        {
            var eventCategory = await _eventCategoryProvider.GetByIdAsync(eventCategoryId);
            var categoryDto = _mapper.Map<CategoriesDto>(eventCategory);
            if(categoryDto != null)
            {
                categoryDto.ThumbnailImagePath = string.Format(_appConfiguration["BaseUrl"] + "{0}", Constants.GetUrl(Constants.InterestThumbnailImagesPath, categoryDto.Photo));
            }
            return categoryDto;
        }

        public async Task<PagedList<CategoriesDto>> GetListAsync(PaginationParams userParams)
        {   
            var eventCategorys =  _eventCategoryProvider.Get(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ProjectTo<CategoriesDto>(_mapper.ConfigurationProvider);
            if(userParams != null)
            {
                foreach (var item in eventCategorys)
                {
                    item.ThumbnailImagePath = string.Format(_appConfiguration["BaseUrl"] + "{0}", Constants.GetUrl(Constants.InterestThumbnailImagesPath, item.Photo));
                   
                }
            }
           
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                eventCategorys = eventCategorys.Where(x => x.Name.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Code.ToLower().Contains(userParams.Filter.ToLower()));
            }
            return await PagedList<CategoriesDto>.CreateAsync(eventCategorys, userParams);
        }

        public async Task<CategoriesDto> InsertEventcategory(CategoriesDto categoryDto)
        {
            if (_eventCategoryProvider.Get(x => x.Code.ToLower().Trim() == categoryDto.Code.ToLower().Trim() && !x.IsDeleted).Any())
            {
                return null;
            }
            categoryDto.CreatedDate = DateTime.UtcNow;
            var category = _mapper.Map<Categories>(categoryDto);
            await _eventCategoryProvider.AddAsync(category);
            var result = _mapper.Map<CategoriesDto>(category);
            return result;
        }

        public async Task<CategoriesDto> UpdateEventcategory(Guid id, CategoriesDto categoryDto)
        {
            if (_eventCategoryProvider.Get(x => x.Code.ToLower().Trim() == categoryDto.Code.ToLower().Trim() && x.Id != id && !x.IsDeleted).Any())
            {
                return null;
            }
            var category = await _eventCategoryProvider.GetByIdAsync(id);
            if (category != null)
            {
                category.Name = categoryDto.Name;
                category.Code = categoryDto.Code;
                category.Photo = categoryDto.Photo;
                category.UpdatedDate = DateTime.UtcNow;
                await _eventCategoryProvider.UpdateAsync(category);
            }
            var result = _mapper.Map<CategoriesDto>(category);
            return result;
        }
    }
}
