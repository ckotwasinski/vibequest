using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class Notifications : FullAuditedEntity<Guid>
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public Guid ToUserId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid? EventId { get; set; }

        public bool IsViewed { get; set; }
    }
}
