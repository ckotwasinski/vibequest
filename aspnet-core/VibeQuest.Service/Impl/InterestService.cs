using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.Service.Impl
{
    public class InterestService : BaseService, IInterestService
    {
        private readonly IInterestProvider _interestProvider;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _appConfiguration;
        private readonly IUserCategoriesProvider _userCategoriesProvider;
        public string apiImageUrl { get => _appConfiguration[Constants.BaseUrl]; }
        public InterestService(IMapper Mapper,
            IInterestProvider interestProvider,
            IWebHostEnvironment hostingEnvironment,
            IConfiguration appConfiguration,
            IUserCategoriesProvider userCategoriesProvider) : base(Mapper)
        {
            _interestProvider = interestProvider;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = appConfiguration;
            _userCategoriesProvider = userCategoriesProvider;
        }

        public async Task<List<CategoriesDto>> GetInterestListAsync()
        {
            var list = await _interestProvider.GetInterestListAsync();
            var res = _mapper.Map<List<CategoriesDto>>(list);
            if (res != null && res.Count > 0)
            {
                foreach (var item in res)
                {
                   item.LargeImagePath  = string.Format(_appConfiguration["BaseUrl"] + "{0}", Constants.GetUrl(Constants.InterestLargeImagesPath, item.Photo));
                   item.ThumbnailImagePath = string.Format(_appConfiguration["BaseUrl"] + "{0}", Constants.GetUrl(Constants.InterestThumbnailImagesPath, item.Photo));
                   item.IconPath = string.Format(_appConfiguration["BaseUrl"] + "{0}", Constants.GetUrl(Constants.IconImagePath, item.Photo.Replace("jpg","svg")));
                   item.WhiteIconPath = string.Format(_appConfiguration["BaseUrl"] + "{0}", Constants.GetUrl(Constants.WhiteIconImagePath, item.Photo.Replace("jpg", "png")));
                }
 
            }
            else
            {
                return null;
            }
            return res;
        }

        public async Task InsertUserInterest(List<Guid> interests, Guid userId)
        {
            foreach (var item in interests)
            {
                await _interestProvider.InsertUserInterestAsync(userId, item);
            }
        }

        public async Task<List<CategoriesDto>> GetInterestListByUserId(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            var list = await _interestProvider.GetUserCategoriesById(userId);
            var res = _mapper.Map<List<CategoriesDto>>(list);
            if (res != null && res.Count > 0)
            {
                foreach (var item in res)
                {
                    item.LargeImagePath = string.Format(baseUrl, Constants.GetUrl(Constants.InterestLargeImagesPath, item.Photo));
                    item.ThumbnailImagePath = string.Format(baseUrl, Constants.GetUrl(Constants.InterestThumbnailImagesPath, item.Photo));
                }

            }
            else
            {
                return null;
            }
            return res;
        }

        public async Task UpdateUserInterest(List<Guid> interests, Guid userId)
        {
            var userCategories = await _userCategoriesProvider.Get(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
            if (userCategories != null && userCategories.Count > 0)
            {
                foreach (var item in userCategories)
                {
                    item.IsDeleted = true;
                    item.DeletedBy = userId;
                    item.DeletedDate = DateTime.UtcNow;
                    await _userCategoriesProvider.UpdateAsync(item);
                }

            }
            if (interests != null && interests.Count > 0)
            {
                foreach (var item in interests)
                {
                    await _interestProvider.InsertUserInterestAsync(userId, item);
                }

            }

        }
    }
}
