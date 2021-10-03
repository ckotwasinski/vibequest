using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Dto
{
    public class EventDetailsDto
    {
        public EventsDto Events { get; set; }
        public int AttendeesCount { get; set; }
        public int FriendsCount { get; set; }
        public bool IsInviteShow { get; set; }
        public List<AttendeesDto> Attendees { get; set; }

        public List<EventMediaDto> EventMedia { get; set; }
    }

    public class AttendeesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
    }
}
