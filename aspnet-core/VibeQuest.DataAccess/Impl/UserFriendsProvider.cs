using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.DataAccess.Impl
{
    public class UserFriendsProvider : Repository<UserFriends, VibeQuestDbContext>, IUserFriendsProvider
    {
        public UserFriendsProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<UserFriendsDto>> GetUserFriends(Guid userId)
        {
            var data = await (from user in _unitOfWork.Context.Users
                              join userFriends in _unitOfWork.Context.UserFriends
                              on user.Id equals userFriends.FriendUserId
                              where userFriends.UserId == userId && userFriends.Status == Constants.FriendRequestStatus.Accept && !userFriends.IsDeleted && !user.IsDeleted
                              select new UserFriendsDto { Id = userFriends.Id, UserId = userFriends.UserId, FriendUserId = userFriends.FriendUserId, Status = userFriends.Status, Name = user.FullName, ProfilePicture = user.ProfilePhoto }).ToListAsync();
            return data;
        }

        public async Task<List<UserFriendsDto>> GetFriendsUsers(Guid userId)
        {
            var data = await (from user in _unitOfWork.Context.Users
                              join userFriends in _unitOfWork.Context.UserFriends
                              on user.Id equals userFriends.UserId
                              where userFriends.FriendUserId == userId && userFriends.Status == Constants.FriendRequestStatus.Accept && !userFriends.IsDeleted && !user.IsDeleted
                              select new UserFriendsDto { Id = userFriends.Id, UserId = userFriends.FriendUserId, FriendUserId = userFriends.UserId, Status = userFriends.Status, Name = user.FullName, ProfilePicture = user.ProfilePhoto }).ToListAsync();
            return data;
        }

        public async Task<UserFriendsDto> GetSearchFriends(string email, Guid userId)
        {
            var user = await _unitOfWork.Context.Users.Where(x => (x.Email.ToLower() == email.ToLower() || x.FullName.Contains(email)) && !x.IsDeleted).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Id == userId)
                {
                    return null;
                }
                UserFriendsDto userFriendsDto = new UserFriendsDto();
                userFriendsDto.Email = email;
                userFriendsDto.Name = user.FullName;
                userFriendsDto.ProfilePicture = user.ProfilePhoto;
                userFriendsDto.UserId = user.Id;
                var userFriends = await _unitOfWork.Context.UserFriends.Where(x => ((x.UserId == userId && x.FriendUserId == user.Id) || (x.UserId == user.Id && x.FriendUserId == userId)) && !x.IsDeleted).FirstOrDefaultAsync();
                if (userFriends != null)
                {
                    if (userFriends.Status == Constants.FriendRequestStatus.Decline)
                    {
                        userFriendsDto.Status = Constants.SearchFriendStatus.Add;
                    }
                    if (userFriends.Status == Constants.FriendRequestStatus.Pending)
                    {
                        userFriendsDto.Id = userFriends.Id;
                        userFriendsDto.Status = Constants.SearchFriendStatus.Cancel;
                    }
                    if (userFriends.Status == Constants.FriendRequestStatus.Accept)
                    {
                        return null;
                    }
                }
                else
                {
                    userFriendsDto.Status = Constants.SearchFriendStatus.Add;
                }
                return userFriendsDto;

            }
            else
            {
                Regex regex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    return null;
                }
                UserFriendsDto userFriendsDto = new UserFriendsDto();
                userFriendsDto.Email = email;
                userFriendsDto.Status = Constants.SearchFriendStatus.Invite;
                var userInvites = await _unitOfWork.Context.UserInvites.Where(x => (x.EmailId == email) && x.UserId == userId && !x.IsDeleted).FirstOrDefaultAsync();
                if (userInvites != null)
                {
                    userFriendsDto.Status = Constants.SearchFriendStatus.InvitationSent;
                }
                return userFriendsDto;
            }
        }

        public async Task<List<Notifications>> GetNotificationsByUserId(Guid userId)
        {
            return await _unitOfWork.Context.Notifications.Where(x => x.ToUserId == userId && !x.IsDeleted).ToListAsync();
        }

        public async Task<EmailTemplates> GetEmailTemplateForInviteFriend()
        {
            return await _unitOfWork.Context.EmailTemplates.Where(x => x.TemplateCode == Constants.AppLocalization.InviteFriend).FirstOrDefaultAsync();
        }

        public async Task InsertUserFriendInviteAsync(UserInvites userInvite)
        {
            await _unitOfWork.Context.UserInvites.AddAsync(userInvite);
        }

        public async Task<List<Users>> GetInviteFriendsListAsync(Guid userId, Guid eventId)
        {
            var data = await (from user in _unitOfWork.Context.Users
                              join userFriend in _unitOfWork.Context.UserFriends
                              on user.Id equals userFriend.FriendUserId
                              join ea in _unitOfWork.Context.EventAttendees
                              on user.Id equals ea.UserId
                              into evattendee
                              from eventAttendee in evattendee.DefaultIfEmpty()
                              where userFriend.UserId == userId && userFriend.Status == Constants.FriendRequestStatus.Accept && !user.IsDeleted && !userFriend.IsDeleted &&
                              (eventAttendee != null ? eventAttendee.IsDeleted == false : true) && (eventAttendee != null ? eventAttendee.EventId != eventId : true)
                              select user).ToListAsync();
            return data;
        }

        public async Task<List<Users>> GetInviteFriendsUserListAsync(Guid userId, Guid eventId)
        {
            var data = await (from user in _unitOfWork.Context.Users
                              join userFriend in _unitOfWork.Context.UserFriends
                              on user.Id equals userFriend.UserId
                              join ea in _unitOfWork.Context.EventAttendees
                              on user.Id equals ea.UserId
                              into evattendee
                              from eventAttendee in evattendee.DefaultIfEmpty()
                              where userFriend.FriendUserId == userId && userFriend.Status == Constants.FriendRequestStatus.Accept && !user.IsDeleted && !userFriend.IsDeleted &&
                              (eventAttendee != null ? eventAttendee.IsDeleted == false : true) && (eventAttendee != null ? eventAttendee.EventId != eventId : true)
                              select user).ToListAsync();
            return data;
        }

        public async Task<List<UserInvites>> InvitedUserList(string email)
        {
            return await _unitOfWork.Context.UserInvites.Where(x => x.EmailId == email && !x.IsDeleted).ToListAsync();
        }

        public async Task UpdateNotificationIsViewed(Guid userId)
        {
            var list = await _unitOfWork.Context.Notifications.Where(x => x.ToUserId == userId && !x.IsViewed && !x.IsDeleted).ToListAsync();
            foreach(var item in list)
            {
                item.IsViewed = true;
                _unitOfWork.Context.Notifications.Update(item);
            }
        }

        public async Task<bool> checkIsNewNotifications(Guid userId)
        {
            bool isNewNotification = false;
            isNewNotification = await _unitOfWork.Context.Notifications.Where(x => x.ToUserId == userId && !x.IsViewed && !x.IsDeleted).AnyAsync();
            if(!isNewNotification)
            {
                isNewNotification = await _unitOfWork.Context.UserFriends.Where(x => x.FriendUserId == userId && x.Status == Constants.FriendRequestStatus.Pending && !x.IsDeleted).AnyAsync();
            }
            return isNewNotification;
        }
    }
}
