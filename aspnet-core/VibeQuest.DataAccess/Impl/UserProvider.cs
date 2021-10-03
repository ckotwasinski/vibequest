using Microsoft.EntityFrameworkCore;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.DataAccess.Infrastructure;
using VibeQuest.Model;
using VibeQuest.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.DataAccess.Impl
{
    public class UserProvider : Repository<Users, VibeQuestDbContext>, IUserProvider
    {
        public UserProvider(IUnitOfWork<VibeQuestDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public bool CheckDuplicateEmail(string Email)
        {
            return _unitOfWork.Context.Users.Where(x => x.Email.ToLower() == Email.ToLower()).Any();
        }

        public async Task<Users> CreateUpdateUser(Users input)
        {
            if (!CheckDuplicateEmail(input.Email))
            {
                input.IsActive = true;
                await _unitOfWork.Context.Users.AddAsync(input);
                UserRoles userRole = new UserRoles();
                userRole.UserId = input.Id;
                if (_unitOfWork.Context.Roles.Where(x => x.Code == Constants.Roles.Sadmin).Count() > 0)
                {
                    var roleId = await _unitOfWork.Context.Roles.Where(x => x.Code == Constants.Roles.Sadmin).Select(x => x.Id).FirstOrDefaultAsync();
                    userRole.RoleId = roleId;
                    await _unitOfWork.Context.UserRoles.AddAsync(userRole);
                }
            }
            else
            {
                var user = _unitOfWork.Context.Users.Where(x => x.Email.ToLower() == input.Email.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    user.FullName = input.FullName;
                    user.ProfilePhoto = input.ProfilePhoto;
                    user.UpdatedDate = DateTime.UtcNow;

                    if (string.IsNullOrEmpty(user.Password))
                    {
                        user.Password = input.Password;
                        user.PasswordSalt = input.PasswordSalt;
                    }
                    if (!string.IsNullOrEmpty(input.SecurityToken))
                    {
                        user.SecurityToken = input.SecurityToken;
                        user.PasswordResetDate = input.PasswordResetDate;
                    }
                    _unitOfWork.Context.Users.Update(user);
                    return user;
                }
            }

            return input;
        }


        public async Task<Users> GetUserById(Guid id)
        {
            var userDetail = await (from users in _dbSet
                             .Where(x => !x.IsDeleted && x.Id == id)
                                    join userRoles in _unitOfWork.Context.UserRoles
                                    on users.Id equals userRoles.UserId
                                    join roles in _unitOfWork.Context.Roles
                                    on userRoles.RoleId equals roles.Id
                                    select new Users
                                    {
                                        Id = users.Id,
                                        Email = users.Email,
                                        FullName = users.FullName,
                                        RoleCode = roles.Code,
                                        ProfilePhoto = users.ProfilePhoto,
                                        IsActive = users.IsActive
                                    }).FirstOrDefaultAsync();
            return userDetail;
        }

        public IQueryable<Users> GetUsersList()
        {
            var userDetail = (from users in _dbSet
                             .Where(x => !x.IsDeleted)
                              join userRoles in _unitOfWork.Context.UserRoles
                              on users.Id equals userRoles.UserId
                              join roles in _unitOfWork.Context.Roles
                              on userRoles.RoleId equals roles.Id
                              orderby users.CreatedDate descending
                              select new Users
                              {
                                  Id = users.Id,
                                  Email = users.Email,
                                  FullName = users.FullName,
                                  RoleName = roles.Name,
                                  ProfilePhoto = users.ProfilePhoto,
                                  IsActive = users.IsActive
                              }).AsQueryable();
            return userDetail;
        }
    }
}
