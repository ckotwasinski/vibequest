using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class EmailHistoryDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string ToEmailAddress { get; set; }

        public string FromEmailAddress { get; set; }

        public string CCEmailAddress { get; set; }

        public string BCCEmailAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentOn { get; set; }

        public int SentBy { get; set; }

        public bool IsSent { get; set; }
    }
}
