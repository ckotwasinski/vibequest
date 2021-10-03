using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class CategoriesDto : FullAuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string ThumbnailImagePath { get; set; }
        public string LargeImagePath { get; set; }
        public string IconPath { get; set; }
        public string WhiteIconPath { get; set; }
    }
}
