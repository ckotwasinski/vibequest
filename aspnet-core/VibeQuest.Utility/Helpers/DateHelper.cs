using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public static class DateHelper
    {
        public static DateTime ToTimeZoneDateTime(this DateTime input, string timezone = "Central Standard Time")
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            return TimeZoneInfo.ConvertTimeFromUtc(input, cstZone);
        }
    }
}
