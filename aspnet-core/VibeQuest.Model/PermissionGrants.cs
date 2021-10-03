using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class PermissionGrants : FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string ProviderName { get; set; }

        [Required]
        public Guid ProviderKey { get; set; }
    }
}
