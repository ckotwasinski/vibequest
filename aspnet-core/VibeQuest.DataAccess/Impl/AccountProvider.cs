using Microsoft.EntityFrameworkCore;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.DataAccess.Impl
{
    public class AccountProvider : Repository<Users, VibeQuestDbContext>, IAccountProvider
    {
        public AccountProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Users> CheckLoginAsync(string email)
        {
            var userDetail = await (from users in _dbSet
                             .Where(x => x.Email == email && x.IsActive && !x.IsDeleted)
                                    join userRoles in _unitOfWork.Context.UserRoles
                                    on users.Id equals userRoles.UserId
                                    join roles in _unitOfWork.Context.Roles
                                    on userRoles.RoleId equals roles.Id
                                    select new Users
                                    {
                                        Id = users.Id,
                                        FullName = users.FullName,
                                        Email = users.Email,
                                        Password = users.Password,
                                        PasswordSalt = users.PasswordSalt,
                                        RoleCode = roles.Code
                                    }).FirstOrDefaultAsync();
            return userDetail;
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            await base.AddAsync(user, true);
            if (user.Id != Guid.Empty)
            {
                var roles = _unitOfWork.Context.Roles.FirstOrDefault(x => x.Code == user.RoleCode);
                UserRoles userRoles = new UserRoles();
                userRoles.RoleId = roles.Id;
                userRoles.UserId = user.Id;
                await _unitOfWork.Context.UserRoles.AddAsync(userRoles);
                return user;
            }
            return null;
        }

        public async Task<EmailTemplates> GetEmailTemplate()
        {
            return await _unitOfWork.Context.EmailTemplates.Where(x => x.TemplateCode == Constants.AppLocalization.ForgotPasswordCode).FirstOrDefaultAsync();
        }
    }
}
