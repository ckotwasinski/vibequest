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
    [Route("email-template")]
    [ApiController]
    [Authorize]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IEmailTemplatesService _emailTemplateService;

        public EmailTemplateController(IEmailTemplatesService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [Authorize(VibeQuestPermissions.EmailTemplate.Default)]
        [Authorize]
        [HttpGet]
        public async Task<PagedList<EmailTemplatesDto>> GetEmailTemplateList([FromQuery] PaginationParams pagination)
        {
            return await _emailTemplateService.GetListAsync(pagination);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<EmailTemplatesDto> GetEmailTemplateById(Guid id)
        {
            var emailTemplate = await _emailTemplateService.GetEmailTemplateById(id);
            return emailTemplate;
        }

        [Authorize(VibeQuestPermissions.EmailTemplate.Create)]
        [HttpPost]
        public async Task<EmailTemplatesDto> CreateEmailTemplate(EmailTemplatesDto emailTemplatesDto)
        {
            var emailTemplate = await _emailTemplateService.InsertEmailTemplate(emailTemplatesDto);
            if (emailTemplate == null)
                throw new UserFriendlyException(Constants.AppLocalization.EmailTemplateAlreadyExist);
            return emailTemplate;
        }

       [Authorize(VibeQuestPermissions.EmailTemplate.Edit)]
        [HttpPut]
        [Route("{id}")]
        public async Task<EmailTemplatesDto> UpdateEmailTemplate(Guid id, EmailTemplatesDto emailTemplatesDto)
        {
            var emailTemplate = await _emailTemplateService.UpdateEmailTemplate(id, emailTemplatesDto);
            if (emailTemplate == null)
                throw new UserFriendlyException(Constants.AppLocalization.EmailTemplateAlreadyExist);
            return emailTemplate;
        }

        [Authorize(VibeQuestPermissions.EmailTemplate.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteEmailTemplate(Guid id)
        {
            await _emailTemplateService.DeleteEmailTemplateById(id);
        }
    }
}
