using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;
using VibeQuest.Service.Helper;

namespace VibeQuest.Service.Contracts
{
    public interface IEventService : IScopedDependency
    {
        Task<PagedList<EventsDto>> GetListAsync(PaginationParams eventParams);

        Task<EventsDto> InsertEvent(EventsDto eventDto);

        Task<EventMediaDto> UploadEventImageAsync(IFormFile image, Guid eventId, Guid userId);
        Task<List<EventsDto>> GetUserEvents(Guid userId);

        Task<EventsDto> UpdateEvent(EventsDto eventDto, Guid eventId);
        Task<List<EventsDto>> GetUpcomingEvents(Guid userId);

        Task<List<EventsDto>> GetPastEvents(Guid userId);

        Task InsertEventAttendees(Guid userId, Guid eventId);

        Task<EventDetailsDto> GetEventDetails(Guid eventId, Guid userId);

        Task<EventDetailsDto> GetEventDetailsById(Guid eventId);

        Task<EventMediaDto> DeleteEventMedia(Guid eventMediaId);

        Task<EventsDto> DeleteEvent(Guid eventId);

        Task<List<NotificationsDto>> InviteFriends(Guid userId, Guid eventId, List<Guid> userFriendsId);

        Task<MapsDto> GetMapEvents(Guid userId);

        Task<DefaultLocationDto> GetDefaultLocationSettings();
    }
}
