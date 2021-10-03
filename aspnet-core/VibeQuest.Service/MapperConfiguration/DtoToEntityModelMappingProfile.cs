using AutoMapper;
using VibeQuest.Dto;
using VibeQuest.Model;

namespace VibeQuest.Service.MapperConfiguration
{
    public class DtoToEntityModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToEntityModelMappings"; }
        }

        public DtoToEntityModelMappingProfile()
        {
            CreateMap<AuditLogsDto, AuditLogs>();
            CreateMap<AuditLogs, AuditLogsDto>();
            CreateMap<CreateUpdateUserDto, Users>();
            CreateMap<RolesDto, Roles>();
            CreateMap<UsersDto, Users>();
            CreateMap<EmailTemplatesDto, EmailTemplates>();
            CreateMap<CommonLookup, CommonLookupDto>();
            CreateMap<CategoriesDto, Categories>();
            CreateMap<EventsDto, Events>();
            CreateMap<EventMediaDto, EventMedia>();
            CreateMap<NotificationsDto, Notifications>();
        }
    }
}