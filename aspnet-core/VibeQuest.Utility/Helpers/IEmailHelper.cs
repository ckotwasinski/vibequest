using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public interface IEmailHelper: IScopedDependency
    {
        bool SendEmail(List<string> To, List<string> CC, List<string> BCC, string Subject, string Body, List<string> Attachments, List<Tuple<byte[], string>> StreamAttachments = null);
    }
}
