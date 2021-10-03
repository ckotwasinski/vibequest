using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public string Order { get; set; }

        public string Filter { get; set; }


    }
}
