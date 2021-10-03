using VibeQuest.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        public IServiceProvider ServiceProvider { get; }

        internal Dictionary<string, PermissionGroupDefinition> Groups { get; }

        internal PermissionDefinitionContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Groups = new Dictionary<string, PermissionGroupDefinition>();
        }

        public virtual PermissionGroupDefinition AddGroup(
            string name,
            string displayName = null)
        {
            Check.NotNull(name, nameof(name));

            if (Groups.ContainsKey(name))
            {
                throw new UserFriendlyException($"There is already an existing permission group with name: {name}");
            }

            return Groups[name] = new PermissionGroupDefinition(name, displayName);
        }

        public virtual PermissionGroupDefinition GetGroup(string name)
        {
            var group = GetGroupOrNull(name);

            if (group == null)
            {
                throw new UserFriendlyException($"Could not find a permission definition group with the given name: {name}");
            }

            return group;
        }

        public virtual PermissionGroupDefinition GetGroupOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        public virtual void RemoveGroup(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                throw new UserFriendlyException($"Not found permission group with name: {name}");
            }

            Groups.Remove(name);
        }

        public virtual PermissionDefinition GetPermissionOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            foreach (var groupDefinition in Groups.Values)
            {
                var permissionDefinition = groupDefinition.GetPermissionOrNull(name);

                if (permissionDefinition != null)
                {
                    return permissionDefinition;
                }
            }

            return null;
        }
    }
}
