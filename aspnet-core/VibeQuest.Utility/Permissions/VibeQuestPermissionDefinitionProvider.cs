using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public class VibeQuestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var systemGroup = context.AddGroup(VibeQuestPermissions.SystemGroup, VibeQuestPermissions.SystemGroup);
            var identityGroup = context.AddGroup(VibeQuestPermissions.IdentityGroup, VibeQuestPermissions.IdentityGroup);

            var auditLog = systemGroup.AddPermission(VibeQuestPermissions.AuditLog.Default, VibeQuestPermissions.AuditLog.Default);
            auditLog.AddChild(VibeQuestPermissions.AuditLog.ActivityLog, VibeQuestPermissions.AuditLog.ActivityLog);
            auditLog.AddChild(VibeQuestPermissions.AuditLog.ErrorLog, VibeQuestPermissions.AuditLog.ErrorLog);

            var emailTemplate = systemGroup.AddPermission(VibeQuestPermissions.EmailTemplate.Default, VibeQuestPermissions.EmailTemplate.Default);
            emailTemplate.AddChild(VibeQuestPermissions.EmailTemplate.Create, VibeQuestPermissions.EmailTemplate.Create);
            emailTemplate.AddChild(VibeQuestPermissions.EmailTemplate.Edit, VibeQuestPermissions.EmailTemplate.Edit);
            emailTemplate.AddChild(VibeQuestPermissions.EmailTemplate.Delete, VibeQuestPermissions.EmailTemplate.Delete);

            var commonLookup = systemGroup.AddPermission(VibeQuestPermissions.CommonLookup.Default, VibeQuestPermissions.CommonLookup.Default);
            commonLookup.AddChild(VibeQuestPermissions.CommonLookup.Create, VibeQuestPermissions.CommonLookup.Create);
            commonLookup.AddChild(VibeQuestPermissions.CommonLookup.Edit, VibeQuestPermissions.CommonLookup.Edit);
            commonLookup.AddChild(VibeQuestPermissions.CommonLookup.Delete, VibeQuestPermissions.CommonLookup.Delete);

            var emailHistory = systemGroup.AddPermission(VibeQuestPermissions.EmailHistory.Default, VibeQuestPermissions.EmailHistory.Default);

            var eventCategory = systemGroup.AddPermission(VibeQuestPermissions.EventCategory.Default, VibeQuestPermissions.EventCategory.Default);
            eventCategory.AddChild(VibeQuestPermissions.EventCategory.Create, VibeQuestPermissions.EventCategory.Create);
            eventCategory.AddChild(VibeQuestPermissions.EventCategory.Edit, VibeQuestPermissions.EventCategory.Edit);
            eventCategory.AddChild(VibeQuestPermissions.EventCategory.Delete, VibeQuestPermissions.EventCategory.Delete);

            var events = systemGroup.AddPermission(VibeQuestPermissions.Event.Default, VibeQuestPermissions.Event.Default);

            var role = identityGroup.AddPermission(VibeQuestPermissions.Role.Default, VibeQuestPermissions.Role.Default);
            role.AddChild(VibeQuestPermissions.Role.Create, VibeQuestPermissions.Role.Create);
            role.AddChild(VibeQuestPermissions.Role.Edit, VibeQuestPermissions.Role.Edit);
            role.AddChild(VibeQuestPermissions.Role.Delete, VibeQuestPermissions.Role.Delete);
            
            var user = identityGroup.AddPermission(VibeQuestPermissions.User.Default, VibeQuestPermissions.User.Default);
            user.AddChild(VibeQuestPermissions.User.Create, VibeQuestPermissions.User.Create);
            user.AddChild(VibeQuestPermissions.User.Edit, VibeQuestPermissions.User.Edit);
            user.AddChild(VibeQuestPermissions.User.Delete, VibeQuestPermissions.User.Delete);
        }
    }
}
