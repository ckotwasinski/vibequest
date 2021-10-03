using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class UserFriendsDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }

        public Guid FriendUserId { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string Email { get; set; }
    }
}
