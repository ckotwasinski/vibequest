using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class UserCategories : FullAuditedEntity<Guid>
    {
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
    }
}
