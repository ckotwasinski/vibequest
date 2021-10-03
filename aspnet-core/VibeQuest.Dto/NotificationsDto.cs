using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class NotificationsDto : FullAuditedEntityDto<Guid>
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public Guid ToUserId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid? EventId { get; set; }
        public string FromUserName { get; set; }
        public string EventName { get; set; }
        public string ProfilePicture { get; set; }
    }
}
