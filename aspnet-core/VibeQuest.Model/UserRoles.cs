using System;
using System.ComponentModel.DataAnnotations;

namespace VibeQuest.Model
{
    public class UserRoles : FullAuditedEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public Guid RoleId { get; set; }
    }
}
