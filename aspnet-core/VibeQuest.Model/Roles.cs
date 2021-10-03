using System;
using System.ComponentModel.DataAnnotations;

namespace VibeQuest.Model
{
    public class Roles : FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
