using AutoMapper;
using VibeQuest.Dto;
using VibeQuest.Model;

namespace VibeQuest.Service.MapperConfiguration
{
    public class EntityModelToDtoMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "EntityModelToDtoMappings"; }
        }

        public EntityModelToDtoMappingProfile()
        {
            CreateMap<Users, UsersDto>();
            CreateMap<Users, CreateUpdateUserDto>();
            CreateMap<Roles, RolesDto>();
            CreateMap<CommonLookupDto, CommonLookup>();
            CreateMap<EmailTemplates, EmailTemplatesDto>();
            CreateMap<EmailHistory, EmailHistoryDto>();
            CreateMap<Categories, CategoriesDto>();
            CreateMap<Events, EventsDto>();
            CreateMap<EventMedia, EventMediaDto>();
            CreateMap<Notifications, NotificationsDto>();
        }
    }
}