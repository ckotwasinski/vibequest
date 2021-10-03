using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.Permissions;

namespace VibeQuest.Api.Controllers
{
    [ApiController]
    [Route("event")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize(VibeQuestPermissions.Event.Default)]
        [HttpGet]
        public async Task<PagedList<EventsDto>> GetEventList([FromQuery] PaginationParams pagination)
        {
            return await _eventService.GetListAsync(pagination);
        }

        [HttpPost]
        [Authorize]
        public async Task<EventsDto> InsertEventAsync(EventsDto input)
        {
            return await _eventService.InsertEvent(input);
        }

        [Authorize]
        [HttpPost]
        [Route("upload-event-media/{eventId}/{userId}")]
        public async Task<EventMediaDto> UploadEventMediaAsync(Guid eventId, Guid userId)
        {
            var formFiles = HttpContext.Request.Form.Files;

            if (formFiles != null && formFiles.Count() == 0)
                throw new UserFriendlyException(Constants.AppLocalization.InvalidRequest);

            var eventMediaDto = await _eventService.UploadEventImageAsync(formFiles[0], eventId, userId);
            return eventMediaDto;
        }

        [HttpGet]
        [Authorize]
        [Route("my-events/{userId}")]
        public async Task<List<EventsDto>> GetUserEventsAsync(Guid userId)
        {
            return await _eventService.GetUserEvents(userId);
        }

        [HttpPost]
        [Authorize]
        [Route("update-event")]
        public async Task<EventsDto> UpdateEventAsync(EventsDto input)
        {
            return await _eventService.UpdateEvent(input, input.Id);
        }

        [HttpGet]
        [Authorize]
        [Route("upcoming-events/{userId}")]
        public async Task<List<EventsDto>> GetUpcomingEventsAsync(Guid userId)
        {
            return await _eventService.GetUpcomingEvents(userId);
        }

        [HttpGet]
        [Authorize]
        [Route("past-events/{userId}")]
        public async Task<List<EventsDto>> GetPastEventsAsync(Guid userId)
        {
            return await _eventService.GetPastEvents(userId);
        }

        [HttpPost]
        [Authorize]
        [Route("event-attendees/{userId}/{eventId}")]
        public async Task InsertEventAttendeesAsync(Guid userId, Guid eventId)
        {
             await _eventService.InsertEventAttendees(userId, eventId);
        }

        [HttpGet]
        [Authorize]
        [Route("event-details/{id}/{userId}")]
        public async Task<EventDetailsDto> GetEventDetailsAsync(Guid id, Guid userId)
        {
            return await _eventService.GetEventDetails(id, userId);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<EventDetailsDto> GetEventDetailsById(Guid id)
        {
            return await _eventService.GetEventDetailsById(id);
        }

        [HttpDelete]
        [Authorize]
        [Route("event-media/{id}")]
        public async Task<EventMediaDto> DeleteEventMediaAsync(Guid id)
        {
            return await _eventService.DeleteEventMedia(id);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<EventsDto> DeleteEventByIdAsync(Guid id)
        {
            return await _eventService.DeleteEvent(id);
        }

        [HttpPost]
        [Authorize]
        [Route("invite-friend-event/{userId}/{eventId}")]
        public async Task<List<NotificationsDto>> InviteFriendEventAsync(Guid userId, Guid eventId, List<Guid> userFriendIds) 
        {
            return await _eventService.InviteFriends(userId, eventId, userFriendIds);
        }

        [HttpGet]
        [Authorize]
        [Route("map-events/{userId}")]
        public async Task<MapsDto> GetMapEvents(Guid userId)
        {
            return await _eventService.GetMapEvents(userId);
        }

        [HttpGet]
        [Authorize]
        [Route("default-location")]
        public async Task<DefaultLocationDto> GetDefaultLocation()
        {
            return await _eventService.GetDefaultLocationSettings();
        }
    }
}
