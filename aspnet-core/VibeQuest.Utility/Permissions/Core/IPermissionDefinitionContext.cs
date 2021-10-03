using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public interface IPermissionDefinitionContext
    {
        //TODO: Add Get methods to find and modify a permission or group.

        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets a pre-defined permission group.
        /// Throws <see cref="AbpException"/> if can not find the given group.
        /// </summary>
        /// <param name="name">Name of the group</param>
        /// <returns></returns>
        PermissionGroupDefinition GetGroup(string name);

        /// <summary>
        /// Tries to get a pre-defined permission group.
        /// Returns null if can not find the given group.
        /// </summary>
        /// <param name="name">Name of the group</param>
        /// <returns></returns>
        PermissionGroupDefinition GetGroupOrNull(string name);

        PermissionGroupDefinition AddGroup(
            string name,
            string displayName = null);

        void RemoveGroup(string name);

        PermissionDefinition GetPermissionOrNull(string name);
    }
}
