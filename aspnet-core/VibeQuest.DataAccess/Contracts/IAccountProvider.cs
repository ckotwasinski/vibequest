using System.Threading.Tasks;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.DataAccess.Contracts
{
    public interface IAccountProvider : IRepository<Users>, IScopedDependency
    {
        Task<Users> CheckLoginAsync(string email);
        Task<Users> CreateUserAsync(Users user);

        Task<EmailTemplates> GetEmailTemplate();
    }
}
