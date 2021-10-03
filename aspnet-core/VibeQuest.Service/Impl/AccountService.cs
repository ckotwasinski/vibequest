using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VibeQuest.DataAccess.Contracts;
using VibeQuest.Dto;
using VibeQuest.Model;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.Service.Impl
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _appConfiguration;
        private readonly IEmailHelper _emailHelper;
        private readonly IEmailHistoryProvider _emailHistoryProvider;
        private readonly IInterestService _interestService;
        private readonly IUserFriendsProvider _userFriendsProvider;
        private readonly IUserFriendsService _userFriendService;
        public string apiImageUrl { get => _appConfiguration[Constants.BaseUrl]; }

        public AccountService(IMapper Mapper,
            IAccountProvider accountProvider,
            IWebHostEnvironment hostingEnvironment,
            IConfiguration appConfiguration,
            IEmailHelper emailHelper,
            IEmailHistoryProvider emailHistoryProvider,
            IInterestService interestService,
            IUserFriendsProvider userFriendsProvider,
            IUserFriendsService userFriendService) : base(Mapper)
        {
            _accountProvider = accountProvider;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = appConfiguration;
            _emailHelper = emailHelper;
            _emailHistoryProvider = emailHistoryProvider;
            _interestService = interestService;
            _userFriendsProvider = userFriendsProvider;
            _userFriendService = userFriendService;
        }

        public async Task<Users> CheckLoginAsync(LoginDto login)
        {
            var userDetail = await _accountProvider.CheckLoginAsync(login.Email);
            if (userDetail != null && (string.IsNullOrEmpty(userDetail.Password) || PBKDF2.ValidatePassword(login.Password, userDetail.PasswordSalt, userDetail.Password)))
                return userDetail;
            return null;
        }

        public async Task<Users> CreateUserAsync(UsersDto user)
        {
            var isExist = _accountProvider.Get(x => x.Email.ToLower() == user.Email.ToLower()).Any();
            if (!isExist)
            {
                bool isEnabledEmailValidation = _appConfiguration.GetValue<bool>("Settings:IsEnabledEmailValidation");
                if(isEnabledEmailValidation)
                {
                    bool isEmailValidated = false;
                    string EmailText = _appConfiguration.GetValue<string>("Settings:EmailValidationText");
                    if(!string.IsNullOrEmpty(EmailText))
                    {
                        var EmailTextArr = EmailText.Split(',');
                        if(EmailTextArr != null && EmailTextArr.Length > 0)
                        {
                            string email = user.Email;
                            for (int i = 0; i < EmailTextArr.Length; i++)
                            {
                                Regex regex = new Regex(@"^[a-zA-Z0-9_.+-]+@" + EmailTextArr[i] + "+$");
                                Match match = regex.Match(email);
                                if(match.Success)
                                {
                                    isEmailValidated = true;
                                    break;
                                }
                            }
                        }
                    }
                    if(!isEmailValidated)
                    {
                        throw new UserFriendlyException(Constants.AppLocalization.InvalidEmail);
                    }
                }
                Users model = _mapper.Map<Users>(user);
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var hashPassword = PBKDF2.HashPassword(model.Password);
                    model.PasswordSalt = hashPassword.Item1;
                    model.Password = hashPassword.Item2;
                }
                model.RoleCode = Constants.Roles.AppUser;
                model = await _accountProvider.CreateUserAsync(model);
                var userInvites = await _userFriendsProvider.InvitedUserList(user.Email);
                if(userInvites != null && userInvites.Count > 0)
                {
                    foreach(var item in userInvites)
                    {
                        await _userFriendService.SendFriendRequest(item.UserId, model.Id);
                    }
                }

                return model;
            }
            return null;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _accountProvider.Get(x => x.Email.ToLower() == email.ToLower() && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            if (user != null)
            {
                Random random = new Random();
                user.TokenExpiryDate = DateTime.UtcNow.AddMinutes(10);
                user.SecurityToken = (random.Next(1000, 9999)).ToString();
                await _accountProvider.UpdateAsync(user);
                //Thread reqRestThread = new Thread(() => SendEmailForForgotPassword(user));
                //reqRestThread.IsBackground = true;
                //reqRestThread.Start();
                await SendEmailForForgotPassword(user);
                return true;
            }
            return false;
        }

        private async Task SendEmailForForgotPassword(Users model)
        {
            var template = await _accountProvider.GetEmailTemplate();
            if (template != null)
            {
                List<string> To = new List<string>();
                List<string> Attachment = new List<string>();
                List<string> BCC = new List<string>();
                List<string> CC = new List<string>();

                string fromEmail = _appConfiguration.GetValue<string>("Settings:MailUsername");
                var bodyDictionary = new Dictionary<string, string> { { "@@Name@@", model.FullName }, { "@@ResetPasswordOtp@@", model.SecurityToken } };

                string subject = template.Subject;
                string body = template.Body.ReplaceWith(bodyDictionary);
                To.Add(model.Email);
                bool mail = false;
                mail = _emailHelper.SendEmail(To, CC, BCC, subject, body, Attachment);
                await _emailHistoryProvider.SaveEmailAsync(To, CC, BCC, subject, body, fromEmail, model.FullName, mail);
            }
        }

        public async Task<bool> VerifyOtpAsync(ResetPasswordVerifyDto input)
        {
            var user = await _accountProvider.Get(x => x.Email.ToLower() == input.Email.ToLower() && x.SecurityToken == input.SecurityToken && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.TokenExpiryDate <= DateTime.UtcNow)
                    throw new UserFriendlyException(Constants.AppLocalization.UserOtpExpired);
                return true;
            }
            return false;
        }

        public async Task<Users> ResetPasswordAsync(ResetPasswordDto input)
        {
            var user = await _accountProvider.Get(x => x.Email == input.Email && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(input.Password))
                {
                    var hashPassword = PBKDF2.HashPassword(input.Password);
                    user.PasswordSalt = hashPassword.Item1;
                    user.Password = hashPassword.Item2;
                    user.SecurityToken = "";
                    await _accountProvider.UpdateAsync(user);
                    user.RoleCode = Constants.Roles.AppUser;
                    return user;
                }
            }
            return null;
        }

        public async Task<UsersDto> UpdateProfileAsync(string name, Guid userId)
        {
            var user = await _accountProvider.Get(x => x.Id == userId && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            if (user != null)
            {
                user.FullName = name;
                await _accountProvider.UpdateAsync(user);
                return _mapper.Map<UsersDto>(user);

            }
            return null;
        }

        public async Task<string> UploadProfileImageAsync(IFormFile image, Guid id)
        {
            string filename = string.Empty;
            var baseUrl = apiImageUrl + "{0}";
            var user = await _accountProvider.Get(x => x.Id == id && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            if (user != null)
            {
                Users model = _mapper.Map<Users>(user);
                if (image != null)
                {
                    filename = await FileHelper.CreateFileAsync(image, _hostingEnvironment.WebRootPath, Constants.UserImages);
                    model.ProfilePhoto = filename;
                    await _accountProvider.UpdateAsync(model);
                    string imagePath = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, model.ProfilePhoto));
                    return imagePath;
                }
            }
            return null;
        }

        public async Task<UserProfileDto> GetProfileDetailsByEmailAsync(string email)
        {
            var baseUrl = apiImageUrl + "{0}";
            var user = await _accountProvider.Get(x => x.Email.ToLower() == email.ToLower() && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            UserProfileDto userProfileDto = new UserProfileDto();
            if(user!= null)
            {
                userProfileDto.Name = user.FullName;
                userProfileDto.Email = user.Email;

                if(!string.IsNullOrEmpty(user.ProfilePhoto))
                  userProfileDto.ProfilePhoto = string.Format(baseUrl, Constants.GetUrl(Constants.UserImages, user.ProfilePhoto));

                userProfileDto.UserId = user.Id;
                var userInterest = await _interestService.GetInterestListByUserId(user.Id);
                if(userInterest != null && userInterest.Count > 0)
                {
                    userProfileDto.Categories = userInterest;
                }
            }
          
            return userProfileDto;
        }
    }
}
