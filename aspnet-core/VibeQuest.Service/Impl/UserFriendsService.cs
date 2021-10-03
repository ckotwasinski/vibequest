using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.Service.Impl
{
    public class UserFriendsService : BaseService, IUserFriendsService
    {
        private readonly IUserFriendsProvider _userFriendsProvider;
        private readonly IConfiguration _appConfiguration;
        private readonly IEmailHelper _emailHelper;
        private readonly IEmailHistoryProvider _emailHistoryProvider;
        private readonly IUserProvider _userProvider;
        private readonly IEventProvider _eventProvider;

        public string apiImageUrl { get => _appConfiguration[Constants.BaseUrl]; }
        public UserFriendsService(IMapper Mapper,
           IUserFriendsProvider userFriendsProvider,
           IConfiguration appConfiguration,
           IEmailHelper emailHelper,
           IEmailHistoryProvider emailHistoryProvider,
           IUserProvider userProvider,
           IEventProvider eventProvider) : base(Mapper)
        {
            _userFriendsProvider = userFriendsProvider;
            _appConfiguration = appConfiguration;
            _emailHelper = emailHelper;
            _emailHistoryProvider = emailHistoryProvider;
            _userProvider = userProvider;
            _eventProvider = eventProvider;
        }

        public async Task<List<UserFriendsDto>> GetUserFriendsList(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            var friendList = new List<UserFriendsDto>();
            var userFriends = await _userFriendsProvider.GetUserFriends(userId);
            var friendsUser = await _userFriendsProvider.GetFriendsUsers(userId);
            friendList = userFriends.Concat(friendsUser).ToList();
            if (friendList != null && friendList.Count > 0)
            {
                foreach (var item in friendList)
                {
                    if (!string.IsNullOrEmpty(item.ProfilePicture))
                        item.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, item.ProfilePicture));
                }
            }
            else
            {
                return null;
            }
            return friendList;
        }

        public async Task<UserFriends> SendFriendRequest(Guid userId, Guid friendUserId)
        {
            UserFriends userFriends = new UserFriends();
            userFriends.UserId = userId;
            userFriends.FriendUserId = friendUserId;
            userFriends.Status = Constants.FriendRequestStatus.Pending;
            userFriends.CreatedDate = DateTime.UtcNow;
            userFriends.CreatedBy = userId;
            await _userFriendsProvider.AddAsync(userFriends);
            return userFriends;

        }

        public async Task<UserInvites> InsertUserFriendInvite(Guid userId, string email)
        {
            UserInvites userInvite = new UserInvites();
            userInvite.UserId = userId;
            userInvite.EmailId = email;
            userInvite.Status = Constants.InviteFriendStatus.Invite;
            userInvite.CreatedDate = DateTime.UtcNow;
            userInvite.CreatedBy = userId;
            await _userFriendsProvider.InsertUserFriendInviteAsync(userInvite);
            return userInvite;

        }

        public async Task<UserFriends> AcceptDeclineFriendRequest(Guid userFriendsId, string status)
        {
            var userFriends = await _userFriendsProvider.Get(x => x.Id == userFriendsId && !x.IsDeleted).FirstOrDefaultAsync();
            if (userFriends != null)
            {
                userFriends.Status = status;
                await _userFriendsProvider.UpdateAsync(userFriends);
                if (status == Constants.FriendRequestStatus.Accept)
                {
                    Notifications notification = new Notifications();
                    notification.Type = Constants.NotificationType.FriendRequest;
                    notification.Status = Constants.NotificationStatus.Accept;
                    notification.FromUserId = userFriends.FriendUserId;
                    notification.ToUserId = userFriends.UserId;
                    notification.CreatedDate = DateTime.UtcNow;
                    notification.CreatedBy = userFriends.FriendUserId;
                    await _eventProvider.InsertNotificationsAsync(notification);
                }
                return userFriends;
            }
            else
            {
                return null;
            }

        }

        public async Task CancelFriendRequest(Guid userFriendsId)
        {
            var userFriends = await _userFriendsProvider.Get(x => x.Id == userFriendsId && x.Status == Constants.FriendRequestStatus.Pending && !x.IsDeleted).FirstOrDefaultAsync();
            if (userFriends != null)
            {
                userFriends.IsDeleted = true;
                await _userFriendsProvider.UpdateAsync(userFriends);
            }
        }

        public async Task UnfriendUser(Guid userId, Guid friendUserId)
        {
            var userFriends = await _userFriendsProvider.Get(x => ((x.UserId == userId && x.FriendUserId == friendUserId) || (x.UserId == friendUserId && x.FriendUserId == userId)) && x.Status == Constants.FriendRequestStatus.Accept && !x.IsDeleted).FirstOrDefaultAsync();
            if (userFriends != null)
            {
                userFriends.IsDeleted = true;
                await _userFriendsProvider.UpdateAsync(userFriends);
            }
        }

        public async Task<UserFriendsDto> GetSearchFriends(string email, Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            var data = await _userFriendsProvider.GetSearchFriends(email, userId);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.ProfilePicture))
                    data.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, data.ProfilePicture));
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<NotificationsListDto>> GetNotificationsList(Guid userId)
        {
            var baseUrl = apiImageUrl + "{0}";
            await _userFriendsProvider.UpdateNotificationIsViewed(userId);
            List<NotificationsListDto> notificationsList = new List<NotificationsListDto>();
            var notifications = await _userFriendsProvider.GetNotificationsByUserId(userId);
            var friendRequests = await _userFriendsProvider.Get(x => x.FriendUserId == userId && x.Status == Constants.FriendRequestStatus.Pending && !x.IsDeleted).ToListAsync();
            if (notifications != null && notifications.Count > 0)
            {
                foreach (var item in notifications)
                {
                    NotificationsListDto notificationListDto = new NotificationsListDto();
                    var notificationDto = _mapper.Map<NotificationsDto>(item);
                    var user = await _userProvider.Get(x => x.Id == item.FromUserId).Select(x => x).FirstOrDefaultAsync();
                    notificationDto.FromUserName = user.FullName;
                    if (item.Type == Constants.NotificationType.Event)
                    {
                        notificationDto.EventName = await _eventProvider.Get(x => x.Id == item.EventId).Select(x => x.Name).FirstOrDefaultAsync();
                    }
                    if (!string.IsNullOrEmpty(user.ProfilePhoto))
                        notificationDto.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, user.ProfilePhoto));

                    notificationListDto.Notifications = notificationDto;
                    notificationListDto.UserFriendsDto = null;
                    notificationListDto.Date = notificationDto.CreatedDate;
                    notificationListDto.Type = "Notification";
                    notificationsList.Add(notificationListDto);
                }
            }
            if (friendRequests != null && friendRequests.Count > 0)
            {
                foreach (var item in friendRequests)
                {
                    NotificationsListDto notificationListDto = new NotificationsListDto();
                    UserFriendsDto userFriendsDto = new UserFriendsDto();
                    userFriendsDto.Id = item.Id;
                    userFriendsDto.UserId = item.UserId;
                    userFriendsDto.FriendUserId = item.FriendUserId;
                    userFriendsDto.Status = item.Status;
                    var user = await _userProvider.Get(x => x.Id == item.UserId && !x.IsDeleted).FirstOrDefaultAsync();
                    userFriendsDto.Name = user.FullName;
                    if (!string.IsNullOrEmpty(user.ProfilePhoto))
                        userFriendsDto.ProfilePicture = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, user.ProfilePhoto));
                    notificationListDto.UserFriendsDto = userFriendsDto;
                    notificationListDto.Notifications = null;
                    notificationListDto.Date = item.CreatedDate;
                    notificationListDto.Type = "FriendRequest";
                    notificationsList.Add(notificationListDto);

                }
            }

            if (notificationsList != null && notificationsList.Count > 0)
            {
                notificationsList = notificationsList.OrderByDescending(x => x.Date).ToList();
                return notificationsList;
            }
            else
            {
                return null;
            }

        }


        public async Task SendEmailForInviteFriend(Guid userId, string email)
        {
            await InsertUserFriendInvite(userId, email);
            var user = await _userProvider.GetByIdAsync(userId);
            var template = await _userFriendsProvider.GetEmailTemplateForInviteFriend();
            if (template != null)
            {
                List<string> To = new List<string>();
                List<string> Attachment = new List<string>();
                List<string> BCC = new List<string>();
                List<string> CC = new List<string>();

                string fromEmail = _appConfiguration.GetValue<string>("Settings:MailUsername");
                var bodyDictionary = new Dictionary<string, string> { { "@@Inviter@@", user.FullName }, { "@@Appdownloadlink@@", _appConfiguration.GetValue<string>("Settings:AppDownloadLink") }, { "@@AppdownloadlinkPlayStore@@", _appConfiguration.GetValue<string>("Settings:AppDownloadLinkPlayStore") } };

                string subject = template.Subject;
                string body = template.Body.ReplaceWith(bodyDictionary);
                To.Add(email);
                bool mail = false;
                mail = _emailHelper.SendEmail(To, CC, BCC, subject, body, Attachment);
                await _emailHistoryProvider.SaveEmailAsync(To, CC, BCC, subject, body, fromEmail, user.FullName, mail);
            }
        }

        public async Task<List<UsersDto>> GetInviteFriendsList(Guid userId,Guid eventId)
        {
            var baseUrl = apiImageUrl + "{0}";
            var friends = await _userFriendsProvider.GetInviteFriendsListAsync(userId, eventId);
            var userFriends = await _userFriendsProvider.GetInviteFriendsUserListAsync(userId, eventId);
            var users = friends.Concat(userFriends).ToList();
            users = users.Distinct().ToList();
            var eventUserId = await _eventProvider.Get(x => x.Id == eventId && !x.IsDeleted).Select(x => x.UserId).FirstOrDefaultAsync();
            if (users != null && users.Count > 0)
            {
                var data = _mapper.Map<List<UsersDto>>(users);
                if(data != null && data.Count > 0)
                {
                    foreach(var item in data)
                    {
                        if (!string.IsNullOrEmpty(item.ProfilePhoto))
                            item.ProfilePhoto = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, item.ProfilePhoto));
                    }
                    data.RemoveAll(x => x.Id == eventUserId);
                    return data;
                }
            }
            return null;
        }

        public async Task<bool> checkIsNewNotifications(Guid userId)
        {
            return await _userFriendsProvider.checkIsNewNotifications(userId);
        }
    }
}
