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
    public class CommonLookupConfiguration : IEntityTypeConfiguration<CommonLookup>
    {
        public void Configure(EntityTypeBuilder<CommonLookup> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
