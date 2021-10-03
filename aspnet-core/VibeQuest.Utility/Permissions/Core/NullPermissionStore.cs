using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions.Core
{
    public class NullPermissionStore : IPermissionStore
    {
        public NullPermissionStore()
        {
        }

        public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            //return TaskCache.FalseResult;
            return Task.FromResult(false);
        }

        public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
        {
            return Task.FromResult(new MultiplePermissionGrantResult(names, PermissionGrantResult.Prohibited));
        }
    }
}
