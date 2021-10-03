using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.Permissions;

namespace VibeQuest.Api.Controllers
{
    [Route("event-category")]
    [ApiController]
    [Authorize]
    public class EventCategoryController : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoryController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
        }

        [Authorize(VibeQuestPermissions.EventCategory.Default)]
        [HttpGet]
        public async Task<PagedList<CategoriesDto>> GetEventCategoryList([FromQuery] PaginationParams pagination)
        {
            return await _eventCategoryService.GetListAsync(pagination);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<CategoriesDto> GetEventCategoryById(Guid id)
        {
            var eventCategory = await _eventCategoryService.GetEventcategoryById(id);
            return eventCategory;
        }

        [Authorize(VibeQuestPermissions.EventCategory.Create)]
        [HttpPost]
        public async Task<CategoriesDto> CreateEventCategory(CategoriesDto categoriesDto)
        {
            var eventCategory = await _eventCategoryService.InsertEventcategory(categoriesDto);
            if (eventCategory == null)
                throw new UserFriendlyException(Constants.AppLocalization.RoleAlreadyExist);
            return eventCategory;
        }

        [Authorize(VibeQuestPermissions.EventCategory.Edit)]
        [HttpPut]
        [Route("{id}")]
        public async Task<CategoriesDto> UpdateEventCategory(Guid id, CategoriesDto categoriesDto)
        {
            var eventCategory = await _eventCategoryService.UpdateEventcategory(id, categoriesDto);
            if (eventCategory == null)
                throw new UserFriendlyException(Constants.AppLocalization.RoleAlreadyExist);
            return eventCategory;
        }

        [Authorize(VibeQuestPermissions.EventCategory.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteEventCategory(Guid id)
        {
            await _eventCategoryService.DeleteEventcategoryById(id);
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/upload-event-category-image")]
        public async Task<string> UploadEventCategoryImageAsync(Guid id)
        {
            var formFiles = HttpContext.Request.Form.Files;

            if (formFiles != null && formFiles.Count() == 0)
                throw new UserFriendlyException(Constants.AppLocalization.InvalidRequest);

            var imagePathDto = await _eventCategoryService.UploadEventCategoryImageAsync(formFiles[0], id);
            return imagePathDto;
        }
    }
}
