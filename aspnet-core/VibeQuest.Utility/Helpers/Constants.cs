using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public class Constants
    {
        public static string BaseUrl = "BaseUrl";
        public const string GrantType = "password";
        public static string InterestLargeImagesPath = "/images/categories/large/{0}";
        public static string InterestThumbnailImagesPath = "/images/categories/thumbnail/{0}";
        public static string EventImagePath = "/images/events/{0}";
        public static string UserImages = "/users/images/{0}";
        public static string IconImagePath = "/images/categories/icons/{0}";
        public static string WhiteIconImagePath = "/images/categories/whiteicons/{0}";

        public static string GetUrl(string path, string filename)
        {
            return string.Format(path, filename);
        }

        public static class Roles
        {
            public static string Sadmin = "sadmin";
            public static string AppUser = "AppUser";
        }

        public static class AppLocalization
        {
            public const string InvalidUser = "Your entered email or password is incorrect!";
            public const string InvalidRequest = "InvalidRequest";
            public const string UserAlreadyExist = "User already exists.";
            public const string UserNotFound = "User not found.";
            public const string JwtBearerDescription = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"";
            public const string ForgotPasswordCode = "FORGOTPASSWORD";
            public const string InviteFriend = "INVITEFRIEND";
            public const string UserOtpExpired = "Your otp is expired.";
            public const string Active = "Active";
            public const string EmailAlreadyExist = "Email already exists.";
            public const string EmailTemplateAlreadyExist = "Email Template code already exists.";
            public const string RoleAlreadyExist = "Role code already exists.";
            public const string CommonLookupAlreadyExist = "Common Lookup already exists.";
            public const string InvalidEmail = "You are not authorized to create account using this email id.";
        }

        public static class ProviderName
        {
            public const string Role = "Role";
            public const string User = "User";
        }

        public static class FriendRequestStatus
        {
            public const string Accept = "Accept";
            public const string Decline = "Decline";
            public const string Pending = "Pending";
        }

        public static class InviteFriendStatus
        {
            public const string Invite = "Invite";
        }

        public static class SearchFriendStatus
        {
            public const string Add = "Add";
            public const string Invite = "Invite";
            public const string Cancel = "Cancel";
            public const string InvitationSent = "Invitation Sent";
        }

        public static class NotificationType
        {
            public const string FriendRequest = "FriendRequest";
            public const string Event = "Event";
        }

        public static class NotificationStatus
        {
            public const string Invite = "Invite";
            public const string Accept = "Accept";
        }

        public static class EventType
        {
            public const string Private = "Private";
            public const string Public = "Public";
        }
    }
}
