using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.Permissions;

namespace VibeQuest.Api.Controllers
{
    [Route("email-history")]
    [ApiController]
    [Authorize]
    public class EmailHistoryController : ControllerBase
    {
        private readonly IEmailHistoryService _emailHistoryService;

        public EmailHistoryController(IEmailHistoryService emailHistoryService)
        {
            _emailHistoryService = emailHistoryService;
        }

       [Authorize(VibeQuestPermissions.EmailHistory.Default)]
        [HttpGet]
        public async Task<PagedList<EmailHistoryDto>> GetEmailHistoryList([FromQuery] PaginationParams pagination)
        {
            return await _emailHistoryService.GetListAsync(pagination);
        }
    }
}
