using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VibeQuest.Model
{
    public class Users : FullAuditedEntity<Guid>
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        [MaxLength(256)]
        public string PasswordSalt { get; set; }

        [MaxLength(500)]
        public string ProfilePhoto { get; set; }

        [MaxLength(100)]
        public string SecurityToken { get; set; }
        public DateTime? TokenExpiryDate { get; set; }

        public DateTime? PasswordResetDate { get; set; }

        [MaxLength(20)]
        public string AuthProvider { get; set; }

        public bool IsActive { get; set; }
        [NotMapped]
        public string RoleCode { get; set; }

        [NotMapped]
        public string RoleName { get; set; }
    }
}
