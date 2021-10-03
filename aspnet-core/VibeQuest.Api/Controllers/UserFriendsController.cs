using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.Api.Controllers
{
    [ApiController]
    [Route("user-friends")]
    public class UserFriendsController : ControllerBase
    {
        private readonly IUserFriendsService _userFriendsService;
        public UserFriendsController(IUserFriendsService userFriendsService)
        {
            _userFriendsService = userFriendsService;
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}")]
        public async Task<List<UserFriendsDto>> GetUserFriendsListAsync(Guid userId)
        {
            return await _userFriendsService.GetUserFriendsList(userId);
        }

        [HttpPost]
        [Authorize]
        [Route("send-friend-request/{userId}/{friendUserId}")]
        public async Task<UserFriends> SendFriendRequestAsync(Guid userId, Guid friendUserId)
        {
            return await _userFriendsService.SendFriendRequest(userId, friendUserId);
        }

        [HttpPost]
        [Authorize]
        [Route("invite-friend/{userId}/{email}")]
        public async Task InviteFriendAsync(Guid userId, string email)
        {
             await _userFriendsService.SendEmailForInviteFriend(userId, email);
        }

        [HttpPost]
        [Authorize]
        [Route("accept-friend-request/{userFriendsId}")]
        public async Task<UserFriends> AcceptFriendRequestAsync(Guid userFriendsId)
        {
            return await _userFriendsService.AcceptDeclineFriendRequest(userFriendsId, Constants.FriendRequestStatus.Accept);
        }

        [HttpPost]
        [Authorize]
        [Route("decline-friend-request/{userFriendsId}")]
        public async Task<UserFriends> DeclineFriendRequestAsync(Guid userFriendsId)
        {
            return await _userFriendsService.AcceptDeclineFriendRequest(userFriendsId, Constants.FriendRequestStatus.Decline);
        }

        [HttpPost]
        [Authorize]
        [Route("cancel-friend-request/{userFriendsId}")]
        public async Task CancelFriendRequestAsync(Guid userFriendsId)
        {
             await _userFriendsService.CancelFriendRequest(userFriendsId);
        }

        [HttpPost]
        [Authorize]
        [Route("unfriend-user/{userId}/{friendUserId}")]
        public async Task UnfriendUserAsync(Guid userId, Guid friendUserId)
        {
             await _userFriendsService.UnfriendUser(userId, friendUserId);
        }

        [HttpGet]
        [Authorize]
        [Route("search-friends/{email}/{userId}")]
        public async Task<UserFriendsDto> GetSearchFriendsAsync(string email, Guid userId)
        {
            return await _userFriendsService.GetSearchFriends(email, userId);
        }

        [HttpGet]
        [Authorize]
        [Route("notifications-list/{userId}")]
        public async Task<List<NotificationsListDto>> GetNotificationsList(Guid userId)
        {
            return await _userFriendsService.GetNotificationsList(userId);
        }

        [HttpGet]
        [Authorize]
        [Route("invite-friends-list/{userId}/{eventId}")]
        public async Task<List<UsersDto>> GetInviteFriendsList(Guid userId, Guid eventId)
        {
            return await _userFriendsService.GetInviteFriendsList(userId,eventId);
        }

        [HttpGet]
        [Authorize]
        [Route("is-new-notifications/{userId}")]
        public async Task<bool> CheckIsNewNotification(Guid userId)
        {
            return await _userFriendsService.checkIsNewNotifications(userId);
        }
    }
}
