using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions.Core
{
    public class PermissionValueProviderManager : IPermissionValueProviderManager
    {
        public IReadOnlyList<IPermissionValueProvider> ValueProviders => _lazyProviders.Value;
        private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

        protected PermissionOptions Options { get; }

        public PermissionValueProviderManager(
            IServiceProvider serviceProvider,
            IOptions<PermissionOptions> options)
        {
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
                    .ToList(),
                true
            );
        }
    }
}
