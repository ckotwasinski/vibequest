using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Helper;
using VibeQuest.Utility.DependencyInjection;

namespace VibeQuest.Service.Contracts
{
    public interface IUserService : IScopedDependency
    {
        Task<PagedList<UsersDto>> GetListAsync(PaginationParams userParams);

        Task<UsersDto> InsertUser(CreateUpdateUserDto userDto);

        Task<UsersDto> UpdateUser(Guid id, CreateUpdateUserDto userDto);

        Task<UsersDto> GetUserById(Guid userId);

        Task DeleteUserById(Guid userId);
    }
}
