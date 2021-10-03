using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.Service.Contracts
{
    public interface IUserFriendsService : IScopedDependency
    {
        Task<List<UserFriendsDto>> GetUserFriendsList(Guid userId);

        Task<UserFriends> SendFriendRequest(Guid userId, Guid friendUserId);

        Task<UserFriends> AcceptDeclineFriendRequest(Guid userFriendsId, string status);

        Task CancelFriendRequest(Guid userFriendsId);

        Task UnfriendUser(Guid userId, Guid friendUserId);

        Task SendEmailForInviteFriend(Guid userId, string email);

        Task<UserFriendsDto> GetSearchFriends(string email, Guid userId);

        Task<List<NotificationsListDto>> GetNotificationsList(Guid userId);

        Task<List<UsersDto>> GetInviteFriendsList(Guid userId, Guid eventId);

        Task<bool> checkIsNewNotifications(Guid userId);


    }
}
