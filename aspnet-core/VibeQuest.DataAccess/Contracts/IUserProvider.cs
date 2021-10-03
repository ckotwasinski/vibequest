using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.DataAccess.Contracts
{
    public interface IUserProvider : IRepository<Users>, IScopedDependency
    {
        bool CheckDuplicateEmail(string Email);

        Task<Users> GetUserById(Guid id);

        IQueryable<Users> GetUsersList();

        Task<Users> CreateUpdateUser(Users input);
    }
}
