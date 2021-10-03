using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class MapsDto
    {
        public List<EventsDto> EventsDto { get; set; }

        public List<CategoriesDto> categoriesDto { get; set; }
    }
}
