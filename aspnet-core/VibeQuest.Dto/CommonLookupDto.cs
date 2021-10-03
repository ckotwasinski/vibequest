using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class CommonLookupDto: FullAuditedEntityDto<Guid>
    {
        public string ConfigName { get; set; }

        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public string Description { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsActive { get; set; }

    }
}
