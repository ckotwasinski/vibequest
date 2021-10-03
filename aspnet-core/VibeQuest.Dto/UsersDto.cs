using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class UsersDto : EntityDto<Guid>
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePhoto { get; set; }

        public string AuthProvider { get; set; }

        public bool IsActive { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool Volunteer { get; set; }
        public string ProfilePhotoFullPath { get; set; }
        public string AppleUserId { get; set; }
        public bool? EmailSubscribed { get; set; }
        public bool? OptOutPassword { get; set; }
        public DateTime? PasswordResetDate { get; set; }
    }

    public class ResetPasswordWebDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
