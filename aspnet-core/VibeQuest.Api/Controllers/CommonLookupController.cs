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
    [ApiController]
    [Route("common-lookup")]
    public class CommonLookupController : ControllerBase
    {
        private readonly ICommonLookupService _commonLookupService;

        public CommonLookupController(ICommonLookupService commonLookupService)
        {
            _commonLookupService = commonLookupService;
        }

        [Authorize(VibeQuestPermissions.CommonLookup.Create)]
        [HttpPost]
        public async Task<CommonLookupDto> CreateAsync(CommonLookupDto commonLookupDto)
        {
            var commonLookup = await _commonLookupService.InsertAsync(commonLookupDto);
            if (commonLookup == null)
                throw new UserFriendlyException(Constants.AppLocalization.CommonLookupAlreadyExist);
            return commonLookup;
        }

        [Authorize(VibeQuestPermissions.CommonLookup.Default)]
        [HttpGet]
        public async Task<PagedList<CommonLookupDto>> GetCommonLookupList([FromQuery] PaginationParams pagination)
        {
            var list = await _commonLookupService.GetListAsync(pagination);
            return list;
        }

        [Authorize(VibeQuestPermissions.CommonLookup.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteCommonLookup(Guid id)
        {
            await _commonLookupService.DeleteCommonLookupById(id);
        }

        [Authorize(VibeQuestPermissions.CommonLookup.Edit)]
        [HttpPut]
        [Route("{id}")]
        public async Task<CommonLookupDto> UpdateCommonLookup(Guid id, CommonLookupDto updateDto)
        {
            var lookup = await _commonLookupService.UpdateCommonLookup(id, updateDto);
            if (lookup == null)
                throw new UserFriendlyException(Constants.AppLocalization.CommonLookupAlreadyExist);
            return lookup;
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<CommonLookupDto> GetCommonLookupById(Guid id)
        {
            return await _commonLookupService.GetCommonLookupById(id);
        }
    }
}
