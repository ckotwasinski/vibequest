using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public interface IPermissionStore : ISingletonDependency
    {
        Task<bool> IsGrantedAsync(
            string name,
            string providerName,
            string providerKey
        );

        Task<MultiplePermissionGrantResult> IsGrantedAsync(
            string[] names,
            string providerName,
            string providerKey
        );
    }
}
