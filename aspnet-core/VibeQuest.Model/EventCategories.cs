using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class EventCategories : FullAuditedEntity<Guid>
    {
        public Guid EventId { get; set; }

        [ForeignKey("EventId")]
        public Events Event { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
    }
}
