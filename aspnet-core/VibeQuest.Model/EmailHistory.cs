using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class EmailHistory : FullAuditedEntity<Guid>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string ToEmailAddress { get; set; }

        [MaxLength(100)]
        public string FromEmailAddress { get; set; }

        [MaxLength(100)]
        public string CCEmailAddress { get; set; }

        [MaxLength(100)]
        public string BCCEmailAddress { get; set; }

        [MaxLength(500)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentOn { get; set; }

        public int SentBy { get; set; }

        public bool IsSent { get; set; }
    }
}
