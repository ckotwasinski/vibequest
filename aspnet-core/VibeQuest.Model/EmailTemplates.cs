using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class EmailTemplates : FullAuditedEntity<Guid>
    {
        [MaxLength(50)]
        [Required]
        public string TemplateCode { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        public string To { get; set; }

        [MaxLength(100)]
        public string CC { get; set; }

        [MaxLength(100)]
        public string BCC { get; set; }

        [MaxLength(500)]
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}
