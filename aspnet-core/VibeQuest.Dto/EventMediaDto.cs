using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class EventMediaDto : FullAuditedEntityDto<Guid>
    {
        public string FileName { get; set; }
        public Guid EventId { get; set; }
    }
}
