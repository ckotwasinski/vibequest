using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeQuest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.DataAccess.Configuration
{
    public class PermissionGrantsConfiguration : IEntityTypeConfiguration<PermissionGrants>
    {
        public void Configure(EntityTypeBuilder<PermissionGrants> builder)
        {
            builder.HasIndex(x => x.ProviderKey);
        }
    }
}
