using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class EventMedia : FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(150)]
        public string FileName { get; set; }
        public Guid EventId { get; set; }

        [ForeignKey("EventId")]
        public Events Event { get; set; }

    }
}
