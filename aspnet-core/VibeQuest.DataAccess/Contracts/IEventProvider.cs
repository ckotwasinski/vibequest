using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.DataAccess.Contracts
{
    public interface IEventProvider : IRepository<Events>, IScopedDependency
    {
        //IQueryable<Events> GetEventsList();

        Task InsertEventCategoryAsync(Guid eventId, Guid categoryId, Guid userId);

        Task<EventMedia> InsertEventMediaAsync(EventMedia eventMedia);

        Task<Categories> GetEventCategoryNameById(Guid eventId);

        Task<EventCategories> GetEventCategoryById(Guid eventId);

        void UpdateEventCategoryAsync(EventCategories eventCategories);

        Task<List<Events>> GetUpcomingEvents(Guid userId);

        Task<List<Events>> GetPastEvents(Guid userId);

        Task InsertEventAttendeesAsync(EventAttendees eventAttendees);

        Task<List<AttendeesDto>> GetEventAttendees(Guid eventId, Guid userId);

        Task<List<AttendeesDto>> GetEventAttendeesFriends(Guid eventId, Guid userId);

        Task<int> GetEventAttendeesCount(Guid eventId);

        Task<List<EventMedia>> GetEventMediaByEventId(Guid eventId);

        Task<EventMedia> GetEventMediaById(Guid eventMediaId);

        void DeleteEventMedia(EventMedia eventMedia);

        Task InsertNotificationsAsync(Notifications notification);

        Task<List<Events>> GetMapEvents(Guid userId);

        Task<bool> IsUserAttendingEvent(Guid userId, Guid eventId);
    }
}
