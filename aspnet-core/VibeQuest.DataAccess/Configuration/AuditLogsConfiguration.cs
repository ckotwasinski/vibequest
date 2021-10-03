using VibeQuest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VibeQuest.DataAccess.Configuration
{
    public class AuditLogsConfiguration : IEntityTypeConfiguration<AuditLogs>
    {
        public void Configure(EntityTypeBuilder<AuditLogs> builder)
        {
            builder.Property(b => b.ClientIpAddress).HasMaxLength(64);
            builder.Property(b => b.BrowserInfo).HasMaxLength(512).IsUnicode(true);
            builder.Property(b => b.HttpMethod).HasMaxLength(16);
            builder.Property(b => b.Url).HasMaxLength(2048);
            builder.Property(b => b.Exception).IsFixedLength(false);
            builder.Property(b => b.Parameters).IsFixedLength(false).IsUnicode(true);
        }
    }
}
