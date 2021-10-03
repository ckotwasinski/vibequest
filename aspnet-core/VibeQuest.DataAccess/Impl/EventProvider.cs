using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.DataAccess.Impl
{
    public class EventProvider : Repository<Events, VibeQuestDbContext>, IEventProvider
    {
        public EventProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public async Task InsertEventCategoryAsync(Guid eventId, Guid categoryId, Guid userId)
        {
            EventCategories eventCategories = new EventCategories();
            eventCategories.EventId = eventId;
            eventCategories.CategoryId = categoryId;
            eventCategories.CreatedBy = userId;
            eventCategories.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.Context.EventCategories.AddAsync(eventCategories);
        }

        //public IQueryable<Events> GetEventsList()
        //{
        //    var userDetail = (from events in _dbSet
        //                     .Where(x => !x.IsDeleted)
        //                      join cate in _unitOfWork.Context.EventCategories
        //                      on events.Id equals userRoles.UserId
        //                      join roles in _unitOfWork.Context.Roles
        //                      on userRoles.RoleId equals roles.Id
        //                      orderby users.CreatedDate descending
        //                      select new Users
        //                      {
        //                          Id = users.Id,
        //                          Email = users.Email,
        //                          FullName = users.FullName,
        //                          RoleName = roles.Name,
        //                          ProfilePhoto = users.ProfilePhoto,
        //                          IsActive = users.IsActive
        //                      }).AsQueryable();
        //    return userDetail;
        //}

        public async Task<EventMedia> InsertEventMediaAsync(EventMedia eventMedia)
        {
            await _unitOfWork.Context.EventMedia.AddAsync(eventMedia);
            return eventMedia;
        }

        public async Task<Categories> GetEventCategoryNameById(Guid eventId)
        {
            var data = await (from category in _unitOfWork.Context.Categories
                              join eventCategory in _unitOfWork.Context.EventCategories
                              on category.Id equals eventCategory.CategoryId
                              where eventCategory.EventId == eventId
                              select category).FirstOrDefaultAsync();
            return data;
        }

        public async Task<EventCategories> GetEventCategoryById(Guid eventId)
        {
            var data = await (from eventCategory in _unitOfWork.Context.EventCategories
                              where eventCategory.EventId == eventId && !eventCategory.IsDeleted
                              select eventCategory).FirstOrDefaultAsync();
            return data;
        }

        public void UpdateEventCategoryAsync(EventCategories eventCategories)
        {
            _unitOfWork.Context.EventCategories.Update(eventCategories);
        }

        public async Task<List<Events>> GetUpcomingEvents(Guid userId)
        {
            var date = DateTime.UtcNow.ToTimeZoneDateTime();

            var data = await (from events in _unitOfWork.Context.Events
                              join eventattend in _unitOfWork.Context.EventAttendees
                              on events.Id equals eventattend.EventId
                              where eventattend.UserId == userId && !eventattend.IsDeleted && !events.IsDeleted && events.Status == Constants.AppLocalization.Active && events.StartTime >= date
                              select events).OrderBy(x => x.StartTime).ToListAsync();
            return data;
        }

        public async Task<List<Events>> GetPastEvents(Guid userId)
        {
            var date = DateTime.UtcNow.ToTimeZoneDateTime();
            var data = await (from events in _unitOfWork.Context.Events
                              join eventattend in _unitOfWork.Context.EventAttendees
                              on events.Id equals eventattend.EventId
                              where eventattend.UserId == userId && !eventattend.IsDeleted && !events.IsDeleted && events.Status == Constants.AppLocalization.Active && events.StartTime < date
                              select events).OrderByDescending(x => x.StartTime).ToListAsync();
            return data;
        }

        public async Task InsertEventAttendeesAsync(EventAttendees eventAttendees)
        {
            await _unitOfWork.Context.EventAttendees.AddAsync(eventAttendees);
        }

        public async Task<List<AttendeesDto>> GetEventAttendees(Guid eventId, Guid userId)
        {
            var data = await (from users in _unitOfWork.Context.Users
                              join eventattend in _unitOfWork.Context.EventAttendees
                              on users.Id equals eventattend.UserId
                              join userFriends in _unitOfWork.Context.UserFriends
                              on eventattend.UserId equals userFriends.FriendUserId
                              where eventattend.EventId == eventId && userFriends.UserId == userId && userFriends.Status == Constants.FriendRequestStatus.Accept && !eventattend.IsDeleted && !users.IsDeleted && !userFriends.IsDeleted
                              select new AttendeesDto { Id = users.Id, Name = users.FullName, ProfilePicture = users.ProfilePhoto, Email = users.Email }).ToListAsync();
            return data;
        }

        public async Task<List<AttendeesDto>> GetEventAttendeesFriends(Guid eventId, Guid userId)
        {
            var data = await (from users in _unitOfWork.Context.Users
                              join eventattend in _unitOfWork.Context.EventAttendees
                              on users.Id equals eventattend.UserId
                              join userFriends in _unitOfWork.Context.UserFriends
                              on eventattend.UserId equals userFriends.UserId
                              where eventattend.EventId == eventId && userFriends.FriendUserId == userId && userFriends.Status == Constants.FriendRequestStatus.Accept && !eventattend.IsDeleted && !users.IsDeleted && !userFriends.IsDeleted
                              select new AttendeesDto { Id = users.Id, Name = users.FullName, ProfilePicture = users.ProfilePhoto, Email = users.Email }).ToListAsync();
            return data;
        }

        public async Task<int> GetEventAttendeesCount(Guid eventId)
        {
            var count = await (from users in _unitOfWork.Context.Users
                              join eventattend in _unitOfWork.Context.EventAttendees
                              on users.Id equals eventattend.UserId
                              where eventattend.EventId == eventId && !eventattend.IsDeleted && !users.IsDeleted
                              select users).CountAsync();
            return count;
        }

        public async Task<List<EventMedia>> GetEventMediaByEventId(Guid eventId)
        {
            return await _unitOfWork.Context.EventMedia.Where(x => x.EventId == eventId && !x.IsDeleted).ToListAsync();
        }

        public async Task<EventMedia> GetEventMediaById(Guid eventMediaId)
        {
            return await _unitOfWork.Context.EventMedia.Where(x => x.Id == eventMediaId && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public void DeleteEventMedia(EventMedia eventMedia)
        {
             _unitOfWork.Context.EventMedia.Update(eventMedia);
        }

        public async Task InsertNotificationsAsync(Notifications notification)
        {
            await _unitOfWork.Context.Notifications.AddAsync(notification);
        }

        public async Task<List<Events>> GetMapEvents(Guid userId)
        {
            var date = DateTime.UtcNow.ToTimeZoneDateTime();

            var publicList = await (from events in _unitOfWork.Context.Events
                                    join eventCategory in _unitOfWork.Context.EventCategories
                                    on events.Id equals eventCategory.EventId
                                    join userCategory in _unitOfWork.Context.UserCategories
                                    on eventCategory.CategoryId equals userCategory.CategoryId
                                    where events.EventType == Constants.EventType.Public && userCategory.UserId == userId  && !events.IsDeleted && !eventCategory.IsDeleted && !userCategory.IsDeleted && events.StartTime >= date
                                    select events).ToListAsync();

            var attendingList = await (from events in _unitOfWork.Context.Events
                                       join eventAttend in _unitOfWork.Context.EventAttendees
                                       on events.Id equals eventAttend.EventId
                                       where eventAttend.UserId == userId && !eventAttend.IsDeleted && !events.IsDeleted && events.StartTime >= date
                                       select events).ToListAsync();

            var myEventList = await (from events in _unitOfWork.Context.Events
                                     where events.UserId == userId && !events.IsDeleted && events.StartTime >= date
                                     select events).ToListAsync();

            var eventsList = publicList.Concat(attendingList).ToList();
            eventsList = eventsList.Concat(myEventList).ToList();
            eventsList = eventsList.Distinct().ToList();
            return eventsList;
        }
        public async Task<bool> IsUserAttendingEvent(Guid userId,Guid eventId)
        {
            return await _unitOfWork.Context.EventAttendees.AnyAsync(x => x.EventId == eventId && x.UserId == userId && !x.IsDeleted);
        }
    }
}
