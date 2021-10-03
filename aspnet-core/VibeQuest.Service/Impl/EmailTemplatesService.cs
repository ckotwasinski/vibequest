using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace VibeQuest.Service.Impl
{
    public class EmailTemplatesService : BaseService, IEmailTemplatesService
    {
        private readonly IEmailTemplatesProvider _emailTemplateProvider;

        public EmailTemplatesService(IMapper Mapper,
           IEmailTemplatesProvider emailTemplateProvider) : base(Mapper)
        {
            _emailTemplateProvider = emailTemplateProvider;
        }
        public async Task<PagedList<EmailTemplatesDto>> GetListAsync(PaginationParams userParams)
        {
            var emailTemplates = _emailTemplateProvider.Get(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ProjectTo<EmailTemplatesDto>(_mapper.ConfigurationProvider);
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                emailTemplates = emailTemplates.Where(x => x.Name.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Subject.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.TemplateCode.ToLower().Contains(userParams.Filter.ToLower())
                );
            }
            return await PagedList<EmailTemplatesDto>.CreateAsync(emailTemplates, userParams);
        }

        public async Task<EmailTemplatesDto> InsertEmailTemplate(EmailTemplatesDto emailTemplateDto)
        {
            if (_emailTemplateProvider.Get(x => x.TemplateCode.ToLower().Trim() == emailTemplateDto.TemplateCode.ToLower().Trim()).Any())
            {
                return null;
            }
            emailTemplateDto.CreatedDate = DateTime.UtcNow;
            var emailTemplate = _mapper.Map<EmailTemplates>(emailTemplateDto);
            await _emailTemplateProvider.AddAsync(emailTemplate);
            var result = _mapper.Map<EmailTemplatesDto>(emailTemplate);
            return result;
        }
        public async Task<EmailTemplatesDto> UpdateEmailTemplate(Guid id, EmailTemplatesDto emailTemplateDto)
        {
            if (_emailTemplateProvider.Get(x => x.TemplateCode.ToLower().Trim() == emailTemplateDto.TemplateCode.ToLower().Trim() && x.Id != id).Any())
            {
                return null;
            }
            var emailTemplate = await _emailTemplateProvider.GetByIdAsync(id);
            if (emailTemplate != null)
            {
                emailTemplate.TemplateCode = emailTemplateDto.TemplateCode;
                emailTemplate.Name = emailTemplateDto.Name;
                emailTemplate.To = emailTemplateDto.To;
                emailTemplate.CC = emailTemplateDto.CC;
                emailTemplate.BCC = emailTemplateDto.BCC;
                emailTemplate.Subject = emailTemplateDto.Subject;
                emailTemplate.Body = emailTemplateDto.Body;
                emailTemplate.IsActive = emailTemplateDto.IsActive;
                emailTemplate.UpdatedDate = DateTime.UtcNow;
                await _emailTemplateProvider.UpdateAsync(emailTemplate);
            }
            var result = _mapper.Map<EmailTemplatesDto>(emailTemplate);
            return result;
        }

        public async Task<EmailTemplatesDto> GetEmailTemplateById(Guid emailTemplateId)
        {
            var emailTemplate = await _emailTemplateProvider.GetByIdAsync(emailTemplateId);
            return _mapper.Map<EmailTemplatesDto>(emailTemplate);

        }
        public async Task DeleteEmailTemplateById(Guid emailTemplateId)
        {
            var emailTemplate = await _emailTemplateProvider.GetByIdAsync(emailTemplateId);
            if (emailTemplate != null)
            {
                emailTemplate.IsDeleted = true;
                emailTemplate.DeletedDate = DateTime.UtcNow;
                await _emailTemplateProvider.UpdateAsync(emailTemplate);
            }
        }
    }
}
