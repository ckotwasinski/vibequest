using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;
using AutoMapper.QueryableExtensions;
using VibeQuest.Service.Helper;

namespace VibeQuest.Service.Impl
{
    public class EventService : BaseService, IEventService
    {
        private readonly IEventProvider _eventProvider;
        private readonly IConfiguration _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IInterestProvider _interestProvider;
        private readonly IUserProvider _userProvider;

        public string apiImageUrl { get => _appConfiguration[Constants.BaseUrl]; }
        public EventService(IMapper Mapper,
            IEventProvider eventProvider,
            IConfiguration appConfiguration,
            IWebHostEnvironment hostingEnvironment,
            IInterestProvider interestProvider,
            IUserProvider userProvider) : base(Mapper)
        {
            _eventProvider = eventProvider;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = appConfiguration;
            _interestProvider = interestProvider;
            _userProvider = userProvider;
        }

        public async Task<PagedList<EventsDto>> GetListAsync(PaginationParams eventParams)
        {
            var baseUrl = apiImageUrl + "{0}";
            var events = _eventProvider.Get(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ProjectTo<EventsDto>(_mapper.ConfigurationProvider);
            if(events != null)
            {
                var user = await _userProvider.Get(x => !x.IsDeleted).ToListAsync();
                foreach(var item in events)
                {
                    item.UserFullName = user.Where(x => x.Id == item.UserId).Select(x =>x.FullName).FirstOrDefault();
                    item.UserEmail = user.Where(x => x.Id == item.UserId).Select(x => x.Email).FirstOrDefault();

                }
            }

            //var events = _eventProvider.GetEventsList().ProjectTo<EventsDto>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(eventParams.Filter))
            {
                events = events.Where(x => x.Name.ToLower().Contains(eventParams.Filter.ToLower())
                );
            }

            return await PagedList<EventsDto>.CreateAsync(events, eventParams);
        }
        public async Task<EventsDto> InsertEvent(EventsDto eventDto)
        {
            var res = _mapper.Map<Events>(eventDto);
            if (res != null)
            {
                var startTimeArr = eventDto.StartTime.Split(":");
                var endTimeArr = eventDto.EndTime.Split(":");
                if (startTimeArr.Length > 0)
                {
                    var startTimeDate = eventDto.Date;
                    if (startTimeDate != null)
                    {
                        startTimeDate = startTimeDate.Value.AddHours(Convert.ToDouble(startTimeArr[0]));
                        startTimeDate = startTimeDate.Value.AddMinutes(Convert.ToDouble(startTimeArr[1]));
                        res.StartTime = startTimeDate;
                    }
                }
                if (endTimeArr.Length > 0)
                {
                    var endTimeDate = eventDto.Date;
                    if (endTimeDate != null)
                    {
                        endTimeDate = endTimeDate.Value.AddHours(Convert.ToDouble(endTimeArr[0]));
                        endTimeDate = endTimeDate.Value.AddMinutes(Convert.ToDouble(endTimeArr[1]));
                        res.EndTime = endTimeDate;
                    }
                }
                res.Status = Constants.AppLocalization.Active;
                await _eventProvider.AddAsync(res, true);
                await _eventProvider.InsertEventCategoryAsync(res.Id, eventDto.CategoryId, eventDto.UserId);
                eventDto.Id = res.Id;
            }
            return eventDto;

        }

        public async Task<EventMediaDto> UploadEventImageAsync(IFormFile image, Guid eventId, Guid userId)
        {
            string filename = string.Empty;
            var baseUrl = apiImageUrl + "{0}";

            if (image != null)
            {
                EventMedia eventMedia = new EventMedia();
                eventMedia.EventId = eventId;
                eventMedia.CreatedBy = userId;
                eventMedia.CreatedDate = DateTime.UtcNow;

                filename = await FileHelper.CreateFileAsync(image, _hostingEnvironment.WebRootPath, Constants.EventImagePath);
                eventMedia.FileName = filename;
                await _eventProvider.InsertEventMediaAsync(eventMedia);
                string imagePath = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, filename));
                return _mapper.Map<EventMediaDto>(eventMedia);
            }

            return null;
        }

        public async Task<List<EventsDto>> GetUserEvents(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            string startTime = "", endTime = "";
            var list = await _eventProvider.Get(x => x.UserId == userId && !x.IsDeleted && x.Status == Constants.AppLocalization.Active).OrderBy(x => x.Date).ToListAsync();
            var eventListDto = new List<EventsDto>();
            foreach (var item in list)
            {
                EventsDto eventDto = new EventsDto();
                eventDto.Id = item.Id;
                eventDto.Name = item.Name;
                eventDto.UserId = item.UserId;
                eventDto.Location = item.Location;
                eventDto.Latitude = item.Latitude;
                eventDto.Longitude = item.Longitude;
                eventDto.EventType = item.EventType;
                if (!string.IsNullOrEmpty(item.Description))
                {
                    eventDto.Description = item.Description;
                }

                if (item.Date != null)
                {
                    eventDto.Date = item.Date;
                }

                if (item.StartTime != null)
                {
                    startTime = item.StartTime.Value.Hour.ToString();
                    startTime += ":" + item.StartTime.Value.Minute.ToString();
                    eventDto.StartTime = startTime;
                }
                if (item.EndTime != null)
                {
                    endTime = item.EndTime.Value.Hour.ToString();
                    endTime += ":" + item.EndTime.Value.Minute.ToString();
                    eventDto.EndTime = endTime;
                }
                var category = await _eventProvider.GetEventCategoryNameById(item.Id);
                string categoryName = category.Name;
                if (!string.IsNullOrEmpty(categoryName))
                {
                    eventDto.CategoryName = categoryName;
                }
                var eventMedia = await _eventProvider.GetEventMediaByEventId(item.Id);
                if (eventMedia != null && eventMedia.Count > 0)
                {
                    eventDto.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, eventMedia[0].FileName));
                }
                else
                {
                    eventDto.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.InterestLargeImagesPath, categoryName.ToLower() + ".jpg"));
                }
                eventListDto.Add(eventDto);

            }
            return eventListDto;
        }

        public async Task<EventsDto> UpdateEvent(EventsDto eventDto, Guid eventId)
        {
            var res = await _eventProvider.Get(x => x.Id == eventId && !x.IsDeleted).FirstOrDefaultAsync();
            if (res != null)
            {
                res.Name = eventDto.Name;
                res.EventType = eventDto.EventType;
                res.Description = eventDto.Description;
                res.Location = eventDto.Location;
                res.Latitude = eventDto.Latitude;
                res.Longitude = eventDto.Longitude;
                res.Date = eventDto.Date;
                res.UserId = eventDto.UserId;
                var startTimeArr = eventDto.StartTime.Split(":");
                var endTimeArr = eventDto.EndTime.Split(":");
                if (startTimeArr.Length > 0)
                {
                    var startTimeDate = eventDto.Date;
                    if (startTimeDate != null)
                    {
                        startTimeDate = startTimeDate.Value.AddHours(Convert.ToDouble(startTimeArr[0]));
                        startTimeDate = startTimeDate.Value.AddMinutes(Convert.ToDouble(startTimeArr[1]));
                        res.StartTime = startTimeDate;
                    }
                }
                if (endTimeArr.Length > 0)
                {
                    var endTimeDate = eventDto.Date;
                    if (endTimeDate != null)
                    {
                        endTimeDate = endTimeDate.Value.AddHours(Convert.ToDouble(endTimeArr[0]));
                        endTimeDate = endTimeDate.Value.AddMinutes(Convert.ToDouble(endTimeArr[1]));
                        res.EndTime = endTimeDate;
                    }
                }
                res.Status = Constants.AppLocalization.Active;
                await _eventProvider.UpdateAsync(res, true);
                var eventCategory = await _eventProvider.GetEventCategoryById(res.Id);
                if (eventCategory != null)
                {
                    eventCategory.CategoryId = eventDto.CategoryId;
                    _eventProvider.UpdateEventCategoryAsync(eventCategory);
                }
            }
            return eventDto;

        }

        public async Task<List<EventsDto>> GetUpcomingEvents(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            string startTime = "", endTime = "";
            var list = await _eventProvider.GetUpcomingEvents(userId);
            var eventListDto = new List<EventsDto>();
            foreach (var item in list)
            {
                EventsDto eventDto = new EventsDto();
                eventDto.Id = item.Id;
                eventDto.Name = item.Name;
                eventDto.UserId = item.UserId;
                eventDto.UserName = await _userProvider.Get(x => x.Id == item.UserId).Select(x => x.FullName).FirstOrDefaultAsync();
                eventDto.Location = item.Location;
                eventDto.Latitude = item.Latitude;
                eventDto.Longitude = item.Longitude;
                eventDto.Status = item.Status;
                eventDto.EventType = item.EventType;
                string profilePicture = await _userProvider.Get(x => x.Id == item.UserId).Select(x => x.ProfilePhoto).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(profilePicture))
                    eventDto.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, profilePicture));
                if (!string.IsNullOrEmpty(item.Description))
                {
                    eventDto.Description = item.Description;
                }

                if (item.Date != null)
                {
                    eventDto.Date = item.Date;
                }

                if (item.StartTime != null)
                {
                    startTime = item.StartTime.Value.Hour.ToString();
                    startTime += ":" + item.StartTime.Value.Minute.ToString();
                    eventDto.StartTime = startTime;
                }
                if (item.EndTime != null)
                {
                    endTime = item.EndTime.Value.Hour.ToString();
                    endTime += ":" + item.EndTime.Value.Minute.ToString();
                    eventDto.EndTime = endTime;
                }
                var category = await _eventProvider.GetEventCategoryNameById(item.Id);
                string categoryName = category.Name;
                if (!string.IsNullOrEmpty(categoryName))
                {
                    eventDto.CategoryName = categoryName;
                }
                var eventMedia = await _eventProvider.GetEventMediaByEventId(item.Id);
                if (eventMedia != null && eventMedia.Count > 0)
                {
                    eventDto.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, eventMedia[0].FileName));
                }
                else
                {
                    eventDto.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.InterestLargeImagesPath, categoryName.ToLower() + ".jpg"));
                }
                eventListDto.Add(eventDto);

            }
            return eventListDto;
        }

        public async Task<List<EventsDto>> GetPastEvents(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            string startTime = "", endTime = "";
            var list = await _eventProvider.GetPastEvents(userId);
            var eventListDto = new List<EventsDto>();
            foreach (var item in list)
            {
                EventsDto eventDto = new EventsDto();
                eventDto.Id = item.Id;
                eventDto.Name = item.Name;
                eventDto.UserId = item.UserId;
                eventDto.Location = item.Location;
                eventDto.Latitude = item.Latitude;
                eventDto.Longitude = item.Longitude;
                eventDto.Status = item.Status;
                eventDto.EventType = item.EventType;
                if (!string.IsNullOrEmpty(item.Description))
                {
                    eventDto.Description = item.Description;
                }

                if (item.Date != null)
                {
                    eventDto.Date = item.Date;
                }

                if (item.StartTime != null)
                {
                    startTime = item.StartTime.Value.Hour.ToString();
                    startTime += ":" + item.StartTime.Value.Minute.ToString();
                    eventDto.StartTime = startTime;
                }
                if (item.EndTime != null)
                {
                    endTime = item.EndTime.Value.Hour.ToString();
                    endTime += ":" + item.EndTime.Value.Minute.ToString();
                    eventDto.EndTime = endTime;
                }
                var category = await _eventProvider.GetEventCategoryNameById(item.Id);
                string categoryName = category.Name;
                if (!string.IsNullOrEmpty(categoryName))
                {
                    eventDto.CategoryName = categoryName;
                }
                var eventMedia = await _eventProvider.GetEventMediaByEventId(item.Id);
                if (eventMedia != null && eventMedia.Count > 0)
                {
                    eventDto.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, eventMedia[0].FileName));
                }
                else
                {
                    eventDto.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.InterestLargeImagesPath, categoryName.ToLower() + ".jpg"));
                }
                eventListDto.Add(eventDto);

            }
            return eventListDto;
        }

        public async Task InsertEventAttendees(Guid userId, Guid eventId)
        {
            var isUserAttendingEvent = await _eventProvider.IsUserAttendingEvent(userId, eventId);
            if(!isUserAttendingEvent)
            {
                EventAttendees eventAttendees = new EventAttendees();
                eventAttendees.UserId = userId;
                eventAttendees.EventId = eventId;
                eventAttendees.CreatedBy = userId;
                eventAttendees.CreatedDate = DateTime.UtcNow;
                await _eventProvider.InsertEventAttendeesAsync(eventAttendees);
            }    

        }

        public async Task<EventDetailsDto> GetEventDetails(Guid eventId, Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            bool isInviteShow = false;
            string startTime = "", endTime = "";
            var item = await _eventProvider.Get(x => x.Id == eventId && !x.IsDeleted).FirstOrDefaultAsync();
            EventDetailsDto eventDetailsDto = new EventDetailsDto();
            EventsDto eventDto = new EventsDto();
            eventDto.Id = item.Id;
            eventDto.Name = item.Name;
            eventDto.UserId = item.UserId;
            eventDto.UserName = await _userProvider.Get(x => x.Id == item.UserId).Select(x => x.FullName).FirstOrDefaultAsync();
            eventDto.Location = item.Location;
            eventDto.Latitude = item.Latitude;
            eventDto.Longitude = item.Longitude;
            eventDto.Status = item.Status;
            eventDto.EventType = item.EventType;
            if (!string.IsNullOrEmpty(item.Description))
            {
                eventDto.Description = item.Description;
            }

            if (item.Date != null)
            {
                eventDto.Date = item.Date;
            }

            if (item.StartTime != null)
            {
                startTime = item.StartTime.Value.Hour.ToString();
                startTime += ":" + item.StartTime.Value.Minute.ToString();
                eventDto.StartTime = startTime;
            }
            if (item.EndTime != null)
            {
                endTime = item.EndTime.Value.Hour.ToString();
                endTime += ":" + item.EndTime.Value.Minute.ToString();
                eventDto.EndTime = endTime;
            }
            string profilePicture = await _userProvider.Get(x => x.Id == item.UserId).Select(x => x.ProfilePhoto).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(profilePicture))
                eventDto.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, profilePicture));
            var category = await _eventProvider.GetEventCategoryNameById(item.Id);
            string categoryName = category.Name;
            if (!string.IsNullOrEmpty(categoryName))
            {
                eventDto.CategoryName = categoryName;
            }
            eventDetailsDto.Events = eventDto;
            if(item.UserId == userId)
            {
                isInviteShow = true;
            }
            var isAttending = await _eventProvider.IsUserAttendingEvent(userId, eventId);
            if (isAttending)
            {
                isInviteShow = true;
            }
            eventDetailsDto.IsInviteShow = isInviteShow;
            var list = new List<AttendeesDto>();
            var eventAttendees = await _eventProvider.GetEventAttendees(eventId, userId);
            var eventAttendeesFriends = await _eventProvider.GetEventAttendeesFriends(eventId, userId);
            eventDetailsDto.AttendeesCount = await _eventProvider.GetEventAttendeesCount(eventId);
            var eventMedia = await _eventProvider.GetEventMediaByEventId(eventId);
            if (eventMedia != null && eventMedia.Count > 0)
            {
                var eventMediaDto = _mapper.Map<List<EventMediaDto>>(eventMedia);
                foreach (var media in eventMediaDto)
                {
                    media.FileName = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, media.FileName));
                }
                eventDetailsDto.EventMedia = eventMediaDto;
            }
            else
            {
                var eventMediaDtoList = new List<EventMediaDto>();
                EventMediaDto mediaEventDto = new EventMediaDto();
                mediaEventDto.FileName = string.Format(baseUrl, Constants.GetUrl(Constants.InterestLargeImagesPath, categoryName.ToLower() + ".jpg"));
                eventMediaDtoList.Add(mediaEventDto);
                eventDetailsDto.EventMedia = eventMediaDtoList;
            }
            list = eventAttendees.Concat(eventAttendeesFriends).ToList();
            //bool isCurrentUserAttend = await _eventProvider.IsUserAttendingEvent(userId, eventId);
            //if(isCurrentUserAttend)
            //{
            //    var userDetails = await _userProvider.Get(x => x.Id == userId).FirstOrDefaultAsync();
            //    if(userDetails != null)
            //    {
            //        AttendeesDto attendeeDto = new AttendeesDto();
            //        attendeeDto.Email = userDetails.Email;
            //        attendeeDto.Id = userId;
            //        attendeeDto.ProfilePicture = userDetails.ProfilePhoto;
            //        attendeeDto.Name = userDetails.FullName;
            //        list.Add(attendeeDto);
            //    }
                
            //}
            if (list != null && list.Count > 0)
            {
                foreach (var attendees in list)
                {
                    if (!string.IsNullOrEmpty(attendees.ProfilePicture))
                        attendees.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, attendees.ProfilePicture));
                }
                eventDetailsDto.Attendees = list;
                eventDetailsDto.FriendsCount = list.Count;
            }
            return eventDetailsDto;

        }

        public async Task<EventDetailsDto> GetEventDetailsById(Guid eventId)
        {
            var baseUrl = apiImageUrl + "{0}";
            string startTime = "", endTime = "";
            var item = await _eventProvider.Get(x => x.Id == eventId && !x.IsDeleted).FirstOrDefaultAsync();
            EventDetailsDto eventDetailsDto = new EventDetailsDto();
            EventsDto eventDto = new EventsDto();
            eventDto.Id = item.Id;
            eventDto.Name = item.Name;
            eventDto.UserId = item.UserId;
            eventDto.Location = item.Location;
            eventDto.Latitude = item.Latitude;
            eventDto.Longitude = item.Longitude;
            eventDto.Status = item.Status;
            eventDto.EventType = item.EventType;
            if (!string.IsNullOrEmpty(item.Description))
            {
                eventDto.Description = item.Description;
            }

            if (item.Date != null)
            {
                eventDto.Date = item.Date;
            }

            if (item.StartTime != null)
            {
                startTime = item.StartTime.Value.Hour.ToString();
                startTime += ":" + item.StartTime.Value.Minute.ToString();
                eventDto.StartTime = startTime;
            }
            if (item.EndTime != null)
            {
                endTime = item.EndTime.Value.Hour.ToString();
                endTime += ":" + item.EndTime.Value.Minute.ToString();
                eventDto.EndTime = endTime;
            }
            var category = await _eventProvider.GetEventCategoryById(eventId);
            if (category != null)
            {
                eventDto.CategoryId = category.CategoryId;
            }
            eventDetailsDto.Events = eventDto;
            var eventMedia = await _eventProvider.GetEventMediaByEventId(eventId);
            if (eventMedia != null && eventMedia.Count > 0)
            {
                var eventMediaDto = _mapper.Map<List<EventMediaDto>>(eventMedia);
                foreach (var media in eventMediaDto)
                {
                    media.FileName = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, media.FileName));
                }
                eventDetailsDto.EventMedia = eventMediaDto;
            }
            return eventDetailsDto;
        }

        public async Task<EventMediaDto> DeleteEventMedia(Guid eventMediaId)
        {
            var eventMedia = await _eventProvider.GetEventMediaById(eventMediaId);
            if (eventMedia != null)
            {
                eventMedia.IsDeleted = true;
                _eventProvider.DeleteEventMedia(eventMedia);
                return _mapper.Map<EventMediaDto>(eventMedia);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<NotificationsDto>> InviteFriends(Guid userId, Guid eventId, List<Guid> userFriendsId)
        {
            if (userFriendsId != null && userFriendsId.Count > 0)
            {
                List<NotificationsDto> list = new List<NotificationsDto>();
                foreach (var userFriendId in userFriendsId)
                {
                    Notifications notification = new Notifications();
                    notification.EventId = eventId;
                    notification.FromUserId = userId;
                    notification.ToUserId = userFriendId;
                    notification.Type = Constants.NotificationType.Event;
                    notification.Status = Constants.NotificationStatus.Invite;
                    notification.CreatedDate = DateTime.UtcNow;
                    notification.CreatedBy = userId;
                    await _eventProvider.InsertNotificationsAsync(notification);
                    var notificationDto = _mapper.Map<NotificationsDto>(notification);
                    list.Add(notificationDto);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public async Task<MapsDto> GetMapEvents(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            var mapsDto = new MapsDto();
            var list = await _eventProvider.GetMapEvents(userId);
            List<Categories> categories = new List<Categories>();
            if(list!=null && list.Count > 0)
            {
                var listDto = _mapper.Map<List<EventsDto>>(list);
                foreach(var item in listDto)
                {
                    var category = await _eventProvider.GetEventCategoryNameById(item.Id);
                    item.CategoryId = category.Id;
                    item.CategoryName = category.Name;
                    categories.Add(category);
                    var eventMedia = await _eventProvider.GetEventMediaByEventId(item.Id);
                    if(!string.IsNullOrEmpty(item.StartTime))
                    {
                        var startTime = item.StartTime.Split(' ');
                        if(startTime != null && startTime.Length > 0)
                        {
                            var start = startTime[1].Split(':');
                            item.StartTime = start[0] +":"+ start[1] + " " + startTime[2];
                        }
                    }
                    if (!string.IsNullOrEmpty(item.EndTime))
                    {
                        var endTime = item.EndTime.Split(' ');
                        if (endTime != null && endTime.Length > 0)
                        {
                            var end = endTime[1].Split(':');
                            item.EndTime = end[0] + ":" + end[1] + " " + endTime[2];
                        }
                    }
                    if (eventMedia != null && eventMedia.Count > 0)
                    {
                        item.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.EventImagePath, eventMedia[0].FileName));
                    }
                    else
                    {
                        item.MediaFileName = string.Format(baseUrl, Constants.GetUrl(Constants.InterestLargeImagesPath, item.CategoryName.ToLower() + ".jpg"));
                    }
                }
                categories = categories.Distinct().ToList();
                mapsDto.EventsDto = listDto;
                if(categories!=null && categories.Count > 0)
                {
                    mapsDto.categoriesDto = _mapper.Map<List<CategoriesDto>>(categories);
                }
                return mapsDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<EventsDto> DeleteEvent(Guid eventId)
        {
            var eventEntity = await _eventProvider.GetByIdAsync(eventId);
            if (eventEntity != null)
            {
                eventEntity.IsDeleted = true;
                await _eventProvider.UpdateAsync(eventEntity);
                return _mapper.Map<EventsDto>(eventEntity);
            }
            else
            {
                return null;
            }
        }

        public async Task<DefaultLocationDto> GetDefaultLocationSettings()
        {
            DefaultLocationDto defaultLocationDto = new DefaultLocationDto();
            defaultLocationDto.Latitude = _appConfiguration.GetValue<string>("Settings:DefaultLatitude");
            defaultLocationDto.Longitude = _appConfiguration.GetValue<string>("Settings:DefaultLongitude");
            return defaultLocationDto;
        }


    }

}

