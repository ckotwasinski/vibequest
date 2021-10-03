using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions.Core
{
    public interface IPermissionValueProviderManager : ISingletonDependency
    {
        IReadOnlyList<IPermissionValueProvider> ValueProviders { get; }
    }
}
