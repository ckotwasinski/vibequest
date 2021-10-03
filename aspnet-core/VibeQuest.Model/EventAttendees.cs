using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class EventAttendees : FullAuditedEntity<Guid>
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }     
        public string Status { get; set; }
    }

}
