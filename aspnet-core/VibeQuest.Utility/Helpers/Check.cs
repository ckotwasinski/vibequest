using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VibeQuest.Utility.Helpers
{
    [DebuggerStepThrough]
    public static class Check
    {
        public static T NotNull<T>(
            T value, 
            string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
