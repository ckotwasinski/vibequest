using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;

namespace VibeQuest.Api.Controllers
{
    [ApiController]
    [Route("interest")]
    public class InterestController : ControllerBase
    {
        private readonly IInterestService _interestService;
        public InterestController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<CategoriesDto>> GetInterestListAsync()
        {
            return await _interestService.GetInterestListAsync();
        }

        [Authorize]
        [HttpPost]
        [Route("user-interest/{userId}")]
        public async Task InsertUserInterest(List<Guid> interests, Guid userId)
        {
            await _interestService.InsertUserInterest(interests, userId);
        }

        [Authorize]
        [HttpGet]
        [Route("user-interest/{userId}")]
        public async Task<List<CategoriesDto>> GetInterestListByUserIdAsync(Guid userId)
        {
            return await _interestService.GetInterestListByUserId(userId);
        }

        [Authorize]
        [HttpPut]
        [Route("update-user-interest/{userId}")]
        public async Task UpdateUserInterest(List<Guid> interests, Guid userId)
        {
            await _interestService.UpdateUserInterest(interests, userId);
        }
    }
}
