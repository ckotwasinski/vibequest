using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class NotificationsListDto
    {
        public NotificationsDto Notifications { get; set; }
        public UserFriendsDto UserFriendsDto { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }

}
