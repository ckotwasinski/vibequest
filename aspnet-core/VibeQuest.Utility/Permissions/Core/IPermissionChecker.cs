using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions.Core
{
    public interface IPermissionChecker : ITransientDependency
    {
        Task<bool> IsGrantedAsync([NotNull] string name);

        Task<bool> IsGrantedAsync([AllowNull] ClaimsPrincipal claimsPrincipal, [NotNull] string name);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([NotNull] string[] names);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([AllowNull] ClaimsPrincipal claimsPrincipal, [NotNull] string[] names);
    }
}
