using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public abstract class PermissionValueProvider : IPermissionValueProvider
    {
        public abstract string Name { get; }

        protected IPermissionStore PermissionStore { get; }

        protected PermissionValueProvider(IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
        }

        public abstract Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);

        public abstract Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context);
    }
}
