using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.JWT;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VibeQuest.Api.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtAuthManager _jwtAuthManager;
        public AccountController(IAccountService accountService, IJwtAuthManager jwtAuthManager)
        {
            _accountService = accountService;
            _jwtAuthManager = jwtAuthManager;
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<UserLoginDto> CheckLoginAsync(LoginDto login)
        {
            var user = await _accountService.CheckLoginAsync(login);
            if (user != null)
                return GenerateUserToken(user);
            else
                throw new UserFriendlyException(Constants.AppLocalization.InvalidUser);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserLoginDto> CreateAsync([FromBody] UsersDto input)
        {
            var user = await _accountService.CreateUserAsync(input);
            if (user == null)
                throw new UserFriendlyException(Constants.AppLocalization.UserAlreadyExist);
            return GenerateUserToken(user);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("forgot-password/{email}")]
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            return await _accountService.ForgotPasswordAsync(email);
        }

        [HttpPost]
        [Route("verify-otp")]
        public async Task<bool> VerifyOtpAsync(ResetPasswordVerifyDto input)
        {
            return await _accountService.VerifyOtpAsync(input);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("reset-password")]
        public async Task<UserLoginDto> ResetPasswordAsync(ResetPasswordDto input)
        {
            var user = await _accountService.ResetPasswordAsync(input);
            if (user == null)
                throw new UserFriendlyException(Constants.AppLocalization.UserNotFound);
            return GenerateUserToken(user);
        }

        [Authorize]
        [HttpPut]
        [Route("update-profile/{userId}/{name}")]
        public async Task<UsersDto> UpdateProfileAsync(Guid userId,string name)
        {
            return await _accountService.UpdateProfileAsync(name, userId);
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/upload-profile-image")]
        public async Task<string> UploadProfileImageAsync(Guid id)
        {
            var formFiles = HttpContext.Request.Form.Files;

            if (formFiles != null && formFiles.Count() == 0)
                throw new UserFriendlyException(Constants.AppLocalization.InvalidRequest);

            var imagePathDto = await _accountService.UploadProfileImageAsync(formFiles[0], id);
            return imagePathDto;
        }

        [Authorize]
        [HttpGet]
        [Route("user-profile/{email}")]
        public async Task<UserProfileDto> GetProfileDetailsByEmailAsync(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _accountService.GetProfileDetailsByEmailAsync(email);
                if (user == null)
                    throw new UserFriendlyException(Constants.AppLocalization.UserNotFound);
                return user;
            }
            else
                throw new UserFriendlyException(Constants.AppLocalization.InvalidRequest);
        }


        private UserLoginDto GenerateUserToken(Users user)
        {
            UserLoginDto userLoginDto = new UserLoginDto();
            var claim = new[]
               {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleCode),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var jwtAuthResult = _jwtAuthManager.GenerateTokens(user.Email, claim, DateTime.UtcNow);
            if (jwtAuthResult != null)
            {
                userLoginDto.Id = user.Id;
                string[] list = user.FullName?.Split(' ');
                userLoginDto.FirstName = list != null && list.Length > 0 ? list[0] : "";
                userLoginDto.LastName = list != null && list.Length > 1 ? list[1] : "";
                userLoginDto.AccessToken = jwtAuthResult.AccessToken;
                userLoginDto.RefreshToken = jwtAuthResult.RefreshToken.TokenString;
                userLoginDto.UserName = jwtAuthResult.RefreshToken.UserName;
                userLoginDto.ExpireAt = jwtAuthResult.RefreshToken.ExpireAt;
            }
            return userLoginDto;
        }
    }
}
