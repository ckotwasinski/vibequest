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
    [Route("audit")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        protected readonly IAuditLogService _auditService;
        public AuditController(IAuditLogService auditLogService)
        {
            _auditService = auditLogService;
        }

        [Route("activity-log")]
        [Authorize(VibeQuestPermissions.AuditLog.ActivityLog)]
        [HttpGet]
        public async Task<PagedList<AuditLogsDto>> GetAuditlogList([FromQuery] PaginationParams pagination)
        {
            return await _auditService.GetListAsync(pagination);
        }

        [Route("error-log")]
        [Authorize(VibeQuestPermissions.AuditLog.ErrorLog)]
        [HttpGet]
        [Authorize]
        public async Task<PagedList<AuditLogsDto>> GetErrorlogList([FromQuery] PaginationParams pagination)
        {
            return await _auditService.GetErrorListAsync(pagination);

        }
    }
}
