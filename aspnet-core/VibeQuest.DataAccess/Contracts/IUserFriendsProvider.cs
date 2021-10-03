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
    public interface IUserFriendsProvider : IRepository<UserFriends>, IScopedDependency
    {
        Task<List<UserFriendsDto>> GetUserFriends(Guid userId);

        Task<List<UserFriendsDto>> GetFriendsUsers(Guid userId);

        Task<UserFriendsDto> GetSearchFriends(string email, Guid userId);

        Task<List<Notifications>> GetNotificationsByUserId(Guid userId);

        Task<EmailTemplates> GetEmailTemplateForInviteFriend();

        Task InsertUserFriendInviteAsync(UserInvites userInvite);

        Task<List<Users>> GetInviteFriendsListAsync(Guid userId, Guid eventId);

        Task<List<Users>> GetInviteFriendsUserListAsync(Guid userId, Guid eventId);

        Task<List<UserInvites>> InvitedUserList(string email);
        Task UpdateNotificationIsViewed(Guid userId);
        Task<bool> checkIsNewNotifications(Guid userId);
    }
}
