using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Model
{
    public class Categories : FullAuditedEntity<Guid>
    {
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public string Photo { get; set; }
    }
}
