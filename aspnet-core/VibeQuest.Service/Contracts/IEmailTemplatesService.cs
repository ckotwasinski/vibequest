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
   public interface IEmailTemplatesService : IScopedDependency
    {
        Task<PagedList<EmailTemplatesDto>> GetListAsync(PaginationParams userParams);

        Task<EmailTemplatesDto> InsertEmailTemplate(EmailTemplatesDto emailTemplateDto);

        Task<EmailTemplatesDto> UpdateEmailTemplate(Guid id, EmailTemplatesDto emailTemplateDto);

        Task<EmailTemplatesDto> GetEmailTemplateById(Guid emailTemplateId);

        Task DeleteEmailTemplateById(Guid emailTemplateId);
    }
}
