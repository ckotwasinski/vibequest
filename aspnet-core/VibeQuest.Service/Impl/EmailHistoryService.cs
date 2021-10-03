using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Service.Helper;

namespace VibeQuest.Service.Impl
{
    public class EmailHistoryService : BaseService, IEmailHistoryService
    {
        private readonly IEmailHistoryProvider _emailHistoryProvider;

        public EmailHistoryService(IMapper Mapper,
            IEmailHistoryProvider emailHistoryProvider) : base(Mapper)
        {
            _emailHistoryProvider = emailHistoryProvider;
        }

        public async Task<PagedList<EmailHistoryDto>> GetListAsync(PaginationParams userParams)
        {
            var emailHistory = _emailHistoryProvider.Get(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ProjectTo<EmailHistoryDto>(_mapper.ConfigurationProvider);
            if (!string.IsNullOrEmpty(userParams.Filter))
            {
                emailHistory = emailHistory.Where(x => x.Name.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.Subject.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.FromEmailAddress.ToLower().Contains(userParams.Filter.ToLower()) ||
                    x.ToEmailAddress.ToLower().Contains(userParams.Filter.ToLower())
                );
            }
            return await PagedList<EmailHistoryDto>.CreateAsync(emailHistory, userParams);
        }
    }
}
