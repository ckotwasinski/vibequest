using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public static class VibeQuestPermissions
    {
        public const string SystemGroup = "System";
        public const string IdentityGroup = "Identity";

        public static class AuditLog
        {
            public const string Default = SystemGroup + ".AuditLog";
            public const string ActivityLog = Default + ".ActivityLog";
            public const string ErrorLog = Default + ".ErrorLog";
        }

        public static class CommonLookup
        {
            public const string Default = SystemGroup + ".CommonLookup";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class EmailHistory
        {
            public const string Default = SystemGroup + ".EmailHistory";
        }

        public static class EmailTemplate
        {
            public const string Default = SystemGroup + ".EmailTemplate";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Event
        {
            public const string Default = SystemGroup + ".Event";
        }

        public static class EventCategory
        {
            public const string Default = SystemGroup + ".EventCategory";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Role
        {
            public const string Default = IdentityGroup + ".Role";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class User
        {
            public const string Default = IdentityGroup + ".User";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
