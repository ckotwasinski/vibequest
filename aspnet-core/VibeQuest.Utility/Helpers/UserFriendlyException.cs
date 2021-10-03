using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    // custom exception class for throwing application specific exceptions 
    // that can be caught and handled within the application
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException() : base() {}

        public UserFriendlyException(string message) : base(message) { }

        public UserFriendlyException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
