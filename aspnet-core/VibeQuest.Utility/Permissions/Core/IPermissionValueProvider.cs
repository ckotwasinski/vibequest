using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public interface IPermissionValueProvider : ITransientDependency
    {
        string Name { get; }

        Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);

        Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context);
    }
}
