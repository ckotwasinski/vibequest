using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions
{
    public interface IPermissionDefinitionProvider : ITransientDependency
    {
        void PreDefine(IPermissionDefinitionContext context);

        void Define(IPermissionDefinitionContext context);

        void PostDefine(IPermissionDefinitionContext context);
    }
}
