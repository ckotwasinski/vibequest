using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.JWT
{
    public interface ICurrentUser : ISingletonDependency
    {
        string Id { get; }
        string Email { get; }
        string Role { get; }
        string Jti { get; }
    }
}
