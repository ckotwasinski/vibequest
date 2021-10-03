using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class EventsDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public DateTime? Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Status { get; set; }

        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string UserName { get; set; }

        public string MediaFileName { get; set; }

        public string EventType { get; set; }

        public string ProfilePicture { get; set; }

        public string UserEmail { get; set; }
        public string UserFullName { get; set; }


    }
}
