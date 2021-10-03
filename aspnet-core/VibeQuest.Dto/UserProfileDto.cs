using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class UserProfileDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Guid UserId { get; set; }

        public string ProfilePhoto { get; set; }

        public List<CategoriesDto> Categories { get; set; }
    }
}
