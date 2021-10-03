using System;
using System.ComponentModel.DataAnnotations;

namespace VibeQuest.Model
{
    public class CommonLookup: FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string ConfigName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ConfigKey { get; set; }

        [Required]
        [MaxLength(100)]
        public string ConfigValue { get; set; }

        public int? DisplayOrder { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
