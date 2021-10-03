using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class UserInvites : FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(150)]
        public string EmailId { get; set; }

        public string Status { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }
    }
}
