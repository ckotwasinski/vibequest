using System;
using System.Collections.Generic;
using System.Text;

namespace VibeQuest.Dto
{
    public class AuditLogsDto : EntityDto<Guid>
    {
        public Guid? UserId { get; set; }
        public DateTime ExecutionTime { get; set; }
        public long ExecutionDuration { get; set; }
        public string ClientIpAddress { get; set; }
        public string BrowserInfo { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string Exception { get; set; }
        public int HttpStatusCode { get; set; }
        public string Comments { get; set; }
        public string Parameters { get; set; }
        public string userName { get; set; }

        public AuditLogsDto(Guid id)
        {
            Id = id;
        }
    }
}
