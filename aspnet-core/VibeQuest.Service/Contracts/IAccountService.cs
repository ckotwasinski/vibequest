using Microsoft.AspNetCore.Http;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Service.Contracts
{
    public interface IAccountService : IScopedDependency
    {
        Task<Users> CheckLoginAsync(LoginDto login);
        Task<Users> CreateUserAsync(UsersDto user);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> VerifyOtpAsync(ResetPasswordVerifyDto input);
        Task<Users> ResetPasswordAsync(ResetPasswordDto input);
        Task<UsersDto> UpdateProfileAsync(string name, Guid userId);
        Task<string> UploadProfileImageAsync(IFormFile image, Guid id);
        Task<UserProfileDto> GetProfileDetailsByEmailAsync(string email);
    }
}
