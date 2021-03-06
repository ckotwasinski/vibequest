using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class CreateUpdateUserDto : FullAuditedEntityDto<Guid>
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string ProfilePhoto { get; set; }

        public string AuthProvider { get; set; }

        public bool IsActive { get; set; }

        public string RoleCode { get; set; }
    }
}
