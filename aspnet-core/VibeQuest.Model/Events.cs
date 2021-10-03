using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class Events : FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Status { get; set; }

        public string EventType { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

    }
}
